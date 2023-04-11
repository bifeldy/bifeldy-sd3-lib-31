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
