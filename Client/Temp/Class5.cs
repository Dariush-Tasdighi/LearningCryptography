//Sign and Verify the Data
//Now we can sign and verify the data using the following code,

//private static byte[] Sign(byte[] data, X509Certificate2 privateKey)
//{
//    if (data == null)
//    {
//        throw new ArgumentNullException("data");
//    }
//    if (privateKey == null)
//    {
//        throw new ArgumentNullException("privateKey");
//    }
//    if (!privateKey.HasPrivateKey)
//    {
//        throw new ArgumentException("invalid certificate", "privateKey");
//    }
//    var provider = (RSACryptoServiceProvider)privateKey.PrivateKey;
//    return provider.SignData(data, new SHA1CryptoServiceProvider());
//}

//private static bool Verify(byte[] data, X509Certificate2 publicKey, byte[] signature)
//{
//    if (data == null)
//    {
//        throw new ArgumentNullException("data");
//    }
//    if (publicKey == null)
//    {
//        throw new ArgumentNullException("publicKey");
//    }
//    if (signature == null)
//    {
//        throw new ArgumentNullException("signature");
//    }
//    var provider = (RSACryptoServiceProvider)publicKey.PublicKey.Key;
//    return provider.VerifyData(data, new SHA1CryptoServiceProvider(), signature);
//}
//Load the Keys
//private static X509Certificate2 LoadPrivateKey()
//{
//    return new X509Certificate2(@"d:\temp\test\MyPFX.pfx", "test");
//}
//private static X509Certificate2 LoadPublicKey()
//{
//    return new X509Certificate2(@"d:\temp\test\MyKey.cer");
//}
//Final Test
//var test = new byte[] { 0x84, 0x73 };
//var sig = Sign(test, LoadPrivateKey());
//Console.WriteLine(Verify(test, LoadPublicKey(), sig));