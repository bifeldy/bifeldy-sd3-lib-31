using System.Threading.Tasks;

namespace bifeldy_sd3_lib_31.Services {

    public interface IApiKeyService {
        Task<bool> CheckKeyOrigin(string ipOrigin, string apiKey);
    }

    public sealed class CApiKeyService : IApiKeyService {

        public CApiKeyService() { }

        public async Task<bool> CheckKeyOrigin(string ipOrigin, string apiKey) {
            return false;
        }

    }

}
