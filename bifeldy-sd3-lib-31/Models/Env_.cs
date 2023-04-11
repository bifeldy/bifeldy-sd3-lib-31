/**
* 
* Author       :: Basilius Bias Astho Christyono
* Mail         :: bias@indomaret.co.id
* Phone        :: (+62) 889 236 6466
* 
* Department   :: IT SD 03
* Mail         :: bias@indomaret.co.id
* 
* Catatan      :: Environment Variable Model
*              :: Tidak Untuk Didaftarkan Ke DI Container
* 
*/

using System;

namespace bifeldy_sd3_lib_31.Models {

    public sealed class Env {

        private string GetEnvVar(string varName) {
            return Environment.GetEnvironmentVariable(varName);
        }

        private bool isUsingPostgres = false;
        public bool IS_USING_POSTGRES {
            get {
                string dbPgEnv = GetEnvVar("IS_USING_POSTGRES");
                if (!string.IsNullOrEmpty(dbPgEnv)) {
                    isUsingPostgres = bool.Parse(dbPgEnv);
                }
                return isUsingPostgres;
            }
            set {
                isUsingPostgres = value;
            } 
        }

        private string kunci = null;
        public string KUNCI {
            get {
                string kunciEnv = GetEnvVar("KUNCI");
                if (!string.IsNullOrEmpty(kunciEnv)) {
                    kunci = kunciEnv;
                }
                return kunci;
            }
            set {
                kunci = value;
            }
        }

        private int maxOldRetentionDay = 0;
        public int MAX_OLD_RETENTION_DAY {
            get {
                string maxOldRetentionDayEnv = GetEnvVar("MAX_OLD_RETENTION_DAY");
                if (!string.IsNullOrEmpty(maxOldRetentionDayEnv)) {
                    maxOldRetentionDay = int.Parse(maxOldRetentionDayEnv);
                }
                return maxOldRetentionDay;
            }
            set {
                maxOldRetentionDay = value;
            }
        }

        private string apiKeyName = null;
        public string API_KEY_NAME {
            get {
                string apiKeyNameEnv = GetEnvVar("API_KEY_NAME");
                if (!string.IsNullOrEmpty(apiKeyNameEnv)) {
                    apiKeyName = apiKeyNameEnv;
                }
                return apiKeyName;
            }
            set {
                apiKeyName = value;
            }
        }

        private string jwtSecret = null;
        public string JWT_SECRET {
            get {
                string jwtSecretEnv = GetEnvVar("JWT_SECRET");
                if (!string.IsNullOrEmpty(jwtSecretEnv)) {
                    jwtSecret = jwtSecretEnv;
                }
                return jwtSecret;
            }
            set {
                jwtSecret = value;
            }
        }

        private string jwtName = null;
        public string JWT_NAME {
            get {
                string jwtEnv = GetEnvVar("API_KEY_NAME");
                if (!string.IsNullOrEmpty(jwtEnv)) {
                    jwtName = jwtEnv;
                }
                return jwtName;
            }
            set {
                jwtName = value;
            }
        }

    }

}
