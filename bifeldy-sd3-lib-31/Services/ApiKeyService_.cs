/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Mail         :: bias@indomaret.co.id
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: Koneksi Ke Database Untuk Cek API Key
 *              :: Harap Didaftarkan Ke DI Container
 * 
 */

using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using bifeldy_sd3_lib_31.Handlers;
using bifeldy_sd3_lib_31.Models;
using bifeldy_sd3_lib_31.Utilities;

namespace bifeldy_sd3_lib_31.Services {

    public interface IApiKeyService {
        Task<bool> CheckKeyOrigin(string ipOrigin, string apiKey);
    }

    public sealed class CApiKeyService : IApiKeyService {

        private readonly IGlobalService _gs;
        private readonly IDbHandler _db;
        private readonly IConverter _converter;

        public CApiKeyService(IGlobalService gs, IDbHandler db, IConverter converter) {
            _gs = gs;
            _db = db;
            _converter = converter;
        }

        public async Task<bool> CheckKeyOrigin(string ipOrigin, string apiKey) {
            DataTable dt = await _db.CheckIpOriginKey(ipOrigin, apiKey);
            List<CMODEL_TABEL_DC_APIKEY_T> ls = _converter.DataTableToList<CMODEL_TABEL_DC_APIKEY_T>(dt);
            List<string> allowed = new List<string>(_gs.allowedIpOrigin);
            if (ls.Count == 1) {
                string ipOriginRec = ls[0].IP_ORIGIN;
                allowed.Add(ipOriginRec == "*" ? ipOrigin : ipOriginRec);
            }
            return allowed.Contains(ipOrigin);
        }

    }

}
