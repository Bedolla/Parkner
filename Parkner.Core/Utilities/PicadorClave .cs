using System;
using System.Security.Cryptography;

namespace Parkner.Core.Utilities
{
    public class PicalConSalResultado
    {
        public static HashSalt GenerarSal(string clave)
        {
            byte[] bytesSal = new byte[64];
            new RNGCryptoServiceProvider().GetNonZeroBytes(bytesSal);

            return new HashSalt {Hash = Convert.ToBase64String(new Rfc2898DeriveBytes(clave, bytesSal, 10000).GetBytes(256)), Salt = Convert.ToBase64String(bytesSal)};
        }

        public static bool VerificarClave(string clave, string hash, string sal) => Convert.ToBase64String(new Rfc2898DeriveBytes(clave, Convert.FromBase64String(sal), 10000).GetBytes(256)).Equals(hash);

        public void Ejemplo()
        {
            HashSalt hashSalt = PicalConSalResultado.GenerarSal("TxtClaveDelUsuario");

            string columnaHash = hashSalt.Hash;
            string columnaSalt = hashSalt.Salt;
        }
    }

    public class HashSalt
    {
        public string Salt { get; set; }
        public string Hash { get; set; }
    }
}
