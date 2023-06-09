﻿/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Mail         :: bias@indomaret.co.id
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: Pengaturan Aplikasi
 *              :: Harap Didaftarkan Ke DI Container
 * 
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;

using bifeldy_sd3_lib_31.Models;

namespace bifeldy_sd3_lib_31.Utilities {

    public interface IApplication {
        Process CurrentProcess { get; }
        bool DebugMode { get; set; }
        string AppName { get; }
        string AppLocation { get; }
        string AppVersion { get; }
        string GetVariabel(string key, string kunci);
        CIpMacAddress[] GetIpMacAddress();
        string[] GetAllIpAddress();
        string[] GetAllMacAddress();
    }

    public class CApplication : IApplication {
        public Process CurrentProcess { get; }
        public bool DebugMode { get; set; } = false;

        private readonly SettingLibb.Class1 _SettingLibb;

        public string AppName { get; }
        public string AppLocation { get; }
        public string AppVersion { get; }

        public CApplication() {
            CurrentProcess = Process.GetCurrentProcess();
            #if DEBUG
                DebugMode = true;
            #endif
            //
            _SettingLibb = new SettingLibb.Class1();
            //
            AppName = Process.GetCurrentProcess().MainModule.ModuleName.ToUpper();
            AppLocation = AppDomain.CurrentDomain.BaseDirectory;
            AppVersion = string.Join("", Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.'));
        }

        public string GetVariabel(string key, string kunci) {
            try {
                // http://xxx.xxx.xxx.xxx/KunciGxxx
                string result = _SettingLibb.GetVariabel(key, kunci);
                if (result.ToUpper().Contains("ERROR")) {
                    throw new Exception("SettingLibb Gagal");
                }
                return result;
            }
            catch {
                return null;
            }
        }

        public CIpMacAddress[] GetIpMacAddress() {
            List<CIpMacAddress> IpMacAddress = new List<CIpMacAddress>();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface nic in nics) {
                if (nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback) {
                    PhysicalAddress mac = nic.GetPhysicalAddress();
                    string iv4 = null;
                    string iv6 = null;
                    IPInterfaceProperties ipInterface = nic.GetIPProperties();
                    foreach (UnicastIPAddressInformation ua in ipInterface.UnicastAddresses) {
                        if (!ua.Address.IsIPv4MappedToIPv6 && !ua.Address.IsIPv6LinkLocal && !ua.Address.IsIPv6Teredo && !ua.Address.IsIPv6SiteLocal) {
                            if (ua.PrefixLength <= 32) {
                                iv4 = ua.Address.ToString();
                            }
                            else if (ua.PrefixLength <= 64) {
                                iv6 = ua.Address.ToString();
                            }
                        }
                    }
                    IpMacAddress.Add(new CIpMacAddress {
                        NAME = nic.Name,
                        DESCRIPTION = nic.Description,
                        MAC_ADDRESS = string.IsNullOrEmpty(mac.ToString()) ? null : mac.ToString(),
                        IP_V4_ADDRESS = iv4,
                        IP_V6_ADDRESS = iv6
                    });
                }
            }
            return IpMacAddress.ToArray();
        }

        public string[] GetAllIpAddress() {
            string[] iv4 = GetIpMacAddress().Where(d => !string.IsNullOrEmpty(d.IP_V4_ADDRESS)).Select(d => d.IP_V4_ADDRESS).ToArray();
            string[] iv6 = GetIpMacAddress().Where(d => !string.IsNullOrEmpty(d.IP_V6_ADDRESS)).Select(d => d.IP_V6_ADDRESS).ToArray();
            string[] ip = new string[iv4.Length + iv6.Length];
            iv4.CopyTo(ip, 0);
            iv6.CopyTo(ip, iv4.Length);
            return ip;
        }

        public string[] GetAllMacAddress() {
            return GetIpMacAddress().Where(d => !string.IsNullOrEmpty(d.MAC_ADDRESS)).Select(d => d.MAC_ADDRESS).ToArray();
        }

    }

}
