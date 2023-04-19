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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

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

        private string kunciIpDomain = "localhost";
        public string KUNCI_IP_DOMAIN {
            get {
                string kunciIpDomainEnv = GetEnvVar("KUNCI_IP_DOMAIN");
                if (!string.IsNullOrEmpty(kunciIpDomainEnv)) {
                    kunciIpDomain = kunciIpDomainEnv;
                }
                return kunciIpDomain;
            }
            set {
                kunciIpDomain = value;
            }
        }

        private string kunciGxxx = "kuncirest";
        public string KUNCI_GXXX {
            get {
                string kunciGxxxEnv = GetEnvVar("KUNCI_GXXX");
                if (!string.IsNullOrEmpty(kunciGxxxEnv)) {
                    kunciGxxx = kunciGxxxEnv;
                }
                return kunciGxxx;
            }
            set {
                kunciGxxx = value;
            }
        }

        private string infoLogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Info_Logs");
        public string INFO_LOG_PATH {
            get {
                string infoLogPathEnv = GetEnvVar("INFO_LOG_PATH");
                if (!string.IsNullOrEmpty(infoLogPathEnv)) {
                    infoLogPath = infoLogPathEnv;
                }
                if (!string.IsNullOrEmpty(infoLogPath) && !RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                    infoLogPath = infoLogPath.Split(":").LastOrDefault();
                }
                return infoLogPath;
            }
            set {
                infoLogPath = value;
            }
        }

        private string errorLogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Error_Logs");
        public string ERROR_LOG_PATH {
            get {
                string errorLogPathEnv = GetEnvVar("ERROR_LOG_PATH");
                if (!string.IsNullOrEmpty(errorLogPathEnv)) {
                    errorLogPath = errorLogPathEnv;
                }
                if (!string.IsNullOrEmpty(errorLogPath) && !RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                    errorLogPath = errorLogPath.Split(":").LastOrDefault();
                }
                return errorLogPath;
            }
            set {
                errorLogPath = value;
            }
        }

        private int maxOldRetentionDay = 14;
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

        private string backupFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backup_Files");
        public string BACKUP_FOLDER_PATH {
            get {
                string backupFolderPathEnv = GetEnvVar("BACKUP_FOLDER_PATH");
                if (!string.IsNullOrEmpty(backupFolderPathEnv)) {
                    backupFolderPath = backupFolderPathEnv;
                }
                if (!string.IsNullOrEmpty(backupFolderPath) && !RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                    backupFolderPath = backupFolderPath.Split(":").LastOrDefault();
                }
                return backupFolderPath;
            }
            set {
                backupFolderPath = value;
            }
        }

        private string tempFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp_Files");
        public string TEMP_FOLDER_PATH {
            get {
                string tempFolderPathEnv = GetEnvVar("TEMP_FOLDER_PATH");
                if (!string.IsNullOrEmpty(tempFolderPathEnv)) {
                    tempFolderPath = tempFolderPathEnv;
                }
                if (!string.IsNullOrEmpty(tempFolderPath) && !RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                    tempFolderPath = tempFolderPath.Split(":").LastOrDefault();
                }
                return tempFolderPath;
            }
            set {
                tempFolderPath = value;
            }
        }

        private string zipFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Zip_Files");
        public string ZIP_FOLDER_PATH {
            get {
                string zipFolderPathEnv = GetEnvVar("ZIP_FOLDER_PATH");
                if (!string.IsNullOrEmpty(zipFolderPathEnv)) {
                    zipFolderPath = zipFolderPathEnv;
                }
                if (!string.IsNullOrEmpty(zipFolderPath) && !RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                    zipFolderPath = zipFolderPath.Split(":").LastOrDefault();
                }
                return zipFolderPath;
            }
            set {
                zipFolderPath = value;
            }
        }

        private string downloadFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Download_Files");
        public string DOWNLOAD_FOLDER_PATH {
            get {
                string downloadFolderPathEnv = GetEnvVar("DOWNLOAD_FOLDER_PATH");
                if (!string.IsNullOrEmpty(downloadFolderPathEnv)) {
                    downloadFolderPath = downloadFolderPathEnv;
                }
                if (!string.IsNullOrEmpty(downloadFolderPath) && !RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                    downloadFolderPath = downloadFolderPath.Split(":").LastOrDefault();
                }
                return downloadFolderPath;
            }
            set {
                downloadFolderPath = value;
            }
        }

        private string apiKeyName = "api_key";
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

        private string jwtSecret = "secret_rahasia";
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

        private string jwtName = "jwt";
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
