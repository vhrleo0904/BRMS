using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Project
{
    class HashConvert
    {
        public string ConvertSha256(string pwd)
        {
            var Sha256 = new SHA256CryptoServiceProvider();
            byte[] ResultHash = Sha256.ComputeHash(Encoding.Default.GetBytes(pwd));
            StringBuilder TransPwd = new StringBuilder();

            foreach (var hash in ResultHash)
            {
                //분리된 문자값 하나로 만드는 작업 수행
                TransPwd.AppendFormat("{0:x2}", hash);
            }
            return TransPwd.ToString();
        }
    }
}
