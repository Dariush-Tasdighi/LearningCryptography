﻿//*By Stephan van Rooij
// * See https://svrooij.nl/2018/04/generate-x509certificate2-in-csharp/
// */

//using System;
//using System.Security.Cryptography.X509Certificates;
//using System.Security.Cryptography;

//using Org.BouncyCastle.X509;
//using Org.BouncyCastle.Utilities;
//using Org.BouncyCastle.Math;
//using Org.BouncyCastle.Security;
//using Org.BouncyCastle.Asn1.X509;
//using Org.BouncyCastle.Crypto;
//using Org.BouncyCastle.Crypto.Generators;
//using Org.BouncyCastle.Crypto.Operators;
//using Org.BouncyCastle.Crypto.Parameters;
//using Org.BouncyCastle.Pkcs;

//namespace Selfsigned
//{
//    public static class Certificates
//    {
//        public static X509Certificate2 GenerateCertificate(string subject)
//        {

//            var random = new SecureRandom();
//            var certificateGenerator = new X509V3CertificateGenerator();

//            var serialNumber = BigIntegers.CreateRandomInRange(BigInteger.One, BigInteger.ValueOf(Int64.MaxValue), random);
//            certificateGenerator.SetSerialNumber(serialNumber);

//            certificateGenerator.SetIssuerDN(new X509Name($"C=NL, O=SomeCompany, CN={subject}"));
//            certificateGenerator.SetSubjectDN(new X509Name($"C=NL, O=SomeCompany, CN={subject}"));
//            certificateGenerator.SetNotBefore(DateTime.UtcNow.Date);
//            certificateGenerator.SetNotAfter(DateTime.UtcNow.Date.AddYears(1));

//            const int strength = 2048;
//            var keyGenerationParameters = new KeyGenerationParameters(random, strength);
//            var keyPairGenerator = new RsaKeyPairGenerator();
//            keyPairGenerator.Init(keyGenerationParameters);

//            var subjectKeyPair = keyPairGenerator.GenerateKeyPair();
//            certificateGenerator.SetPublicKey(subjectKeyPair.Public);

//            var issuerKeyPair = subjectKeyPair;
//            const string signatureAlgorithm = "SHA256WithRSA";
//            var signatureFactory = new Asn1SignatureFactory(signatureAlgorithm, issuerKeyPair.Private);
//            var bouncyCert = certificateGenerator.Generate(signatureFactory);

//            // Lets convert it to X509Certificate2
//            X509Certificate2 certificate;

//            Pkcs12Store store = new Pkcs12StoreBuilder().Build();
//            store.SetKeyEntry($"{subject}_key", new AsymmetricKeyEntry(subjectKeyPair.Private), new[] { new X509CertificateEntry(bouncyCert) });
//            string exportpw = Guid.NewGuid().ToString("x");

//            using (var ms = new System.IO.MemoryStream())
//            {
//                store.Save(ms, exportpw.ToCharArray(), random);
//                certificate = new X509Certificate2(ms.ToArray(), exportpw, X509KeyStorageFlags.Exportable);
//            }

//            //Console.WriteLine($"Generated cert with thumbprint {certificate.Thumbprint}");
//            return certificate;
//        }

//        // You can also load the certificate from to CurrentUser store
//        private X509Certificate2 LoadCertificate(string subject)
//        {
//            var userStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
//            userStore.Open(OpenFlags.OpenExistingOnly);
//            if (userStore.IsOpen)
//            {
//                var collection = userStore.Certificates.Find(X509FindType.FindBySubjectName, Environment.MachineName, false);
//                if (collection.Count > 0)
//                {
//                    return collection[0];
//                }
//                userStore.Close();
//            }
//            return null;
//        }

//        // Or store the certificate to the local store.
//        private void SaveCertificate(X509Certificate2 certificate)
//        {
//            var userStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
//            userStore.Open(OpenFlags.ReadWrite);
//            userStore.Add(certificate);
//            userStore.Close();
//        }
//    }
//}