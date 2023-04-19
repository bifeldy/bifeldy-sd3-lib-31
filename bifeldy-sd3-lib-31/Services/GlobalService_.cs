/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Mail         :: bias@indomaret.co.id
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: Default Service
 *              :: Harap Didaftarkan Ke DI Container
 * 
 */

using System.Collections.Generic;

namespace bifeldy_sd3_lib_31.Services {

    public interface IGlobalService {
        List<string> allowedIpOrigin { get; set; }
        string CleanIpOrigin(string ipOrigin);
    }

    public sealed class CGlobalService : IGlobalService {

        public List<string> allowedIpOrigin { get; set; } = new List<string>() { "localhost", "127.0.0.1" };

        public CGlobalService() { }

        public string CleanIpOrigin(string ipOrigin) {
            ipOrigin ??= "";
            // Remove Prefixes
            if (ipOrigin.StartsWith("::ffff:")) {
                ipOrigin = ipOrigin.Substring(7);
            }
            if (ipOrigin.StartsWith("http://")) {
                ipOrigin = ipOrigin.Substring(7);
            }
            else if (ipOrigin.StartsWith("https://")) {
                ipOrigin = ipOrigin.Substring(8);
            }
            if (ipOrigin.StartsWith("www.")) {
                ipOrigin = ipOrigin.Substring(4);
            }
            // Get Domain Or IP Maybe With Port Included And Remove Folder Path
            ipOrigin = ipOrigin.Split("/")[0];
            // Remove Port
            int totalColon = 0;
            for (int i = 0; i < ipOrigin.Length; i++) {
                if (ipOrigin[i] == ':') {
                    totalColon++;
                }
                if (totalColon > 1) {
                    break;
                }
            }
            if (totalColon == 1) {
                // IPv4
                ipOrigin = ipOrigin.Split(":")[0];
            }
            else {
                // IPv6
                ipOrigin = ipOrigin.Split("]")[0];
                if (ipOrigin.StartsWith("[")) {
                    ipOrigin = ipOrigin.Substring(1);
                }
            }
            return ipOrigin;
        }

    }

}
