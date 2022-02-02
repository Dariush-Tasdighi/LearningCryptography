//public class Class1
//{
//    public Class1()
//    {
//        var cert = new X509Certificate2(...);
//        RSA privateKey = cert.GetRSAPrivateKey();
//        // use the key
//    }
//}
//or

//public class Class1
//{
//    public Class1()
//    {
//        const String RSA = "1.2.840.113549.1.1.1";
//        const String DSA = "1.2.840.10040.4.1";
//        const String ECC = "1.2.840.10045.2.1";
//        var cert = new X509Certificate2(...);
//        switch (cert.PublicKey.Oid.Value)
//        {
//            case RSA:
//                RSA rsa = cert.GetRSAPrivateKey(); // or cert.GetRSAPublicKey() when need public key
//                                                   // use the key
//                break;
//            case DSA:
//                DSA dsa = cert.GetDSAPrivateKey(); // or cert.GetDSAPublicKey() when need public key
//                                                   // use the key
//                break;
//            case ECC:
//                ECDsa ecc = cert.GetECDsaPrivateKey(); // or cert.GetECDsaPublicKey() when need public key
//                                                       // use the key
//                break;
//        }
//    }
//}