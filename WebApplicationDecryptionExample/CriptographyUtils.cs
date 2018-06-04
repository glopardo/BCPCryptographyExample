using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System;

namespace BCP.Security
{
    public static class CriptographyUtils
    {
        /// <summary>
        /// Encrypts input data with the RSA algorithm using the private key from the specified certificate
        /// </summary>
        /// <param name="dataToEncrypt"></param>
        /// <param name="certificateThumbPrint"></param>
        /// <returns></returns>
        public static byte[] Encrypt(this byte[] dataToEncrypt, string certificateThumbPrint)
        {
            var x509CertificatesStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);

            x509CertificatesStore.Open(OpenFlags.MaxAllowed);
            var certCollection = x509CertificatesStore.Certificates.Find(X509FindType.FindByThumbprint, certificateThumbPrint, false);

            if (certCollection.Count == 0)
                throw new Exception(string.Format("El certificado con el thumbprint {0} no existe en el store \"LocalMachine\". Por favor instale el certificado.", certificateThumbPrint));

            byte[] encryptedData;
            using (var rsa = new RSACryptoServiceProvider(512))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(certCollection[0].GetRSAPublicKey().ExportParameters(false));
                encryptedData = rsa.Encrypt(dataToEncrypt, true);
            }

            x509CertificatesStore.Close();
            return encryptedData;
        }

        /// <summary>
        /// Decrypts input data using the RSA algorithm using the private key from the specified certificate
        /// </summary>
        /// <param name="dataToDecrypt"></param>
        /// <param name="certificateThumbPrint"></param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] dataToDecrypt, string certificateThumbPrint)
        {
            var x509CertificatesStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);

            x509CertificatesStore.Open(OpenFlags.MaxAllowed);
            var certCollection = x509CertificatesStore.Certificates.Find(X509FindType.FindByThumbprint, certificateThumbPrint, false);

            if (certCollection.Count == 0)
                throw new Exception(string.Format("El certificado con el thumbprint {0} no existe en el store \"LocalMachine\". Por favor instale el certificado.", certificateThumbPrint));

            byte[] decryptedData;

            using (var rsa = new RSACryptoServiceProvider(512))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(certCollection[0].GetRSAPrivateKey().ExportParameters(true));
                decryptedData = rsa.Decrypt(dataToDecrypt, true);
            }

            x509CertificatesStore.Close();
            return decryptedData;
        }
    }
}
