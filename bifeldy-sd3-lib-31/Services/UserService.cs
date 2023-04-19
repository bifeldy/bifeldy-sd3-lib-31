/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Mail         :: bias@indomaret.co.id
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: Koneksi Ke Database Berkaitan Dengan User
 *              :: Harap Didaftarkan Ke DI Container
 *              :: Bisa Untuk Inherit
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bifeldy_sd3_lib_31.Services {

    public interface IUserService {
        Task<object> GetById(int id);
    }

    public class CUserService : IUserService {

        public CUserService() { }

        public async Task<object> GetById(int id) {
            return null;
        }

    }

}
