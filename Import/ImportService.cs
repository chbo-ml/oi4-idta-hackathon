using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using AasCore.Aas3_0;
using hackathon.Database;
using hackathon.Database.Model;
using hackathon.Utils;
using IO.Swagger.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace hackathon.Import
{
    public class ImportService
    {
        private readonly HackathonContext _hackathonContext;

        public ImportService(HackathonContext hackathonContext)
        {
            _hackathonContext = hackathonContext;
        }

        public async Task ImportFromRepository(string decodedLocalUrl, string decodedRemoteUrl, string decodedId)
        {
            var client = new HttpClient();
            using HttpResponseMessage response = await client.GetAsync(decodedRemoteUrl + $"/shells/{decodedId.ToBase64()}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var shell = Jsonization.Deserialize.AssetAdministrationShellFrom(JsonNode.Parse(responseBody));
            var submodels = new List<Submodel>();

            foreach (var smRef in shell.Submodels ?? [])
            {
                using var resp = await client.GetAsync(decodedRemoteUrl + $"/submodels/{smRef.Keys?[0]?.Value.ToBase64()}");
                resp.EnsureSuccessStatusCode();
                responseBody = await resp.Content.ReadAsStringAsync();

                var sm = Jsonization.Deserialize.SubmodelFrom(JsonNode.Parse(responseBody));
                submodels.Add(sm);
            }
            // Id umschreiben
            if (shell.DerivedFrom == null) {
                shell.DerivedFrom = new Reference(ReferenceTypes.ExternalReference, new List<IKey>(), null);
            }
            shell.DerivedFrom.Keys.Add(new Key(KeyTypes.AssetAdministrationShell, shell.Id));
            shell.Id = Guid.NewGuid().ToString();

            await PushNewToLocalRepositoryAsync(shell, submodels, decodedLocalUrl);
            await PushNewToLocalRegistryAsync(shell, submodels, decodedLocalUrl);
            // await PushNewToLocalDiscoveryAsync(shell, submodels, decodedLocalUrl);

            ImportedShell importedShell = new()
            {
                RemoteRegistryUrl = decodedRemoteUrl
            };

            _hackathonContext.Add(importedShell);
            _hackathonContext.SaveChanges();
        }

        public async Task PushNewToLocalRegistryAsync(AssetAdministrationShell shell, List<Submodel> submodels, string localRegistryUrl)
        {
            var jsonString = CreateShellDescriptorString(shell, submodels, localRegistryUrl);

            var client = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, $"{localRegistryUrl}/shell-descriptors");

            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            request.Content = content;
            var result = await client.SendAsync(request);
            var resultContent = await result.Content.ReadAsStringAsync();

            Console.WriteLine(resultContent);

        }

        public async Task PushNewToLocalDiscoveryAsync(AssetAdministrationShell shell, List<Submodel> submodels, string localRegistryUrl)
        {
            // var jsonString = CreateShellDescriptorString(shell, submodels, localRegistryUrl);

            // var client = new HttpClient();
            // using var request = new HttpRequestMessage(HttpMethod.Post, $"{localRegistryUrl}/registry/shell-descriptors");

            // var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            // request.Content = content;
            // var result = await client.SendAsync(request);
            // var resultContent = await result.Content.ReadAsStringAsync();

            // Console.WriteLine(resultContent);

        }

        public async Task PushNewToLocalRepositoryAsync(AssetAdministrationShell shell, List<Submodel> submodels, string localRepositoryUrl)
        {
            var jsonString = Jsonization.Serialize.ToJsonObject(shell).ToJsonString();

            var client = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, $"{localRepositoryUrl}/shells");

            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            request.Content = content;
            var result = await client.SendAsync(request);
            var resultContent = await result.Content.ReadAsStringAsync();

            Console.WriteLine(resultContent);

        }


        private string CreateShellDescriptorString(AssetAdministrationShell aas, List<Submodel> submodels, string localRegistryUrl)
        {
            var shellEndpoint = $"{localRegistryUrl}/shells/{aas.Id.ToBase64()}";

            var iEndpoint = new IO.Swagger.Model.Endpoint()
            {
                _Interface = "AssetAdministrationShell",
                ProtocolInformation = new ProtocolInformation()
                {
                    Href = shellEndpoint
                }
            };

            var endpointList = new List<IO.Swagger.Model.Endpoint>
        {
            iEndpoint
        };

            var submodelDescriptors = new List<SubmodelDescriptor>();

            submodels.ForEach(sm =>
            {
                submodelDescriptors.Add(CreateSubmodelDescriptors(sm, shellEndpoint));
            });

            var aasDescriptor = new AssetAdministrationShellDescriptor()
            {
                Administration = (AdministrativeInformation?)aas.Administration ?? new AdministrativeInformation(),
                AssetKind = aas.AssetInformation.AssetKind,
                AssetType = aas.AssetInformation.AssetType ?? aas.AssetInformation.AssetKind.ToString() ?? string.Empty,
                Description = aas.Description ?? [],
                DisplayName = aas.DisplayName ?? [],
                Endpoints = endpointList,
                Extensions = aas.Extensions ?? [],
                GlobalAssetId = aas.AssetInformation.GlobalAssetId ?? string.Empty,
                Id = aas.Id,
                IdShort = aas.IdShort ?? string.Empty,
                SpecificAssetIds = aas.AssetInformation.SpecificAssetIds ?? [],
                SubmodelDescriptors = submodelDescriptors
            };

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented,
                Converters = new List<JsonConverter>
                        {
                            new StringEnumConverter()
                        },
                NullValueHandling = NullValueHandling.Ignore
            };
            return JsonConvert.SerializeObject(aasDescriptor, serializerSettings);
        }

        private SubmodelDescriptor CreateSubmodelDescriptors(Submodel sm, string shellEndpoint)
        {
            var iEndpoint = new IO.Swagger.Model.Endpoint()
            {
                _Interface = "Submodel",
                ProtocolInformation = new ProtocolInformation()
                {
                    Href = $"{shellEndpoint}/submodels/{sm.Id.ToBase64()}"
                }
            };

            var endpointList = new List<IO.Swagger.Model.Endpoint>
                {
                    iEndpoint
                };

            var submodelDescriptor = new SubmodelDescriptor()
            {
                Administration = sm.Administration ?? new AdministrativeInformation(),
                Description = sm.Description ?? [],
                DisplayName = sm.DisplayName ?? [],
                Endpoints = endpointList,
                Extensions = sm.Extensions ?? [],
                IdShort = sm.IdShort ?? string.Empty,
                Id = sm.Id,
                SemanticId = sm.SemanticId ?? new Reference(ReferenceTypes.ExternalReference, []),
                SupplementalSemanticId = sm.SupplementalSemanticIds ?? []
            };

            return submodelDescriptor;
        }

    }
}