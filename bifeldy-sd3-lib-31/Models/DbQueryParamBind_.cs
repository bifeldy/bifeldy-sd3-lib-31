﻿/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Mail         :: bias@indomaret.co.id
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: Query Binding Dengan Parameter Turunan `DbParameter`
 *              :: Tidak Untuk Didaftarkan Ke DI Container
 * 
 */

using System.Data;

namespace bifeldy_sd3_lib_31.Models {

    public sealed class CDbQueryParamBind {
        public string NAME { get; set; }
        public dynamic VALUE { get; set; }
        public int SIZE { get; set; }
        public ParameterDirection DIRECTION { get; set; }
    }

}
