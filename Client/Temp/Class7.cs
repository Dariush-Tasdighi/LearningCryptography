//private enum KeyFileKinds
//{
//    None = 0,
//    Pkcs8,
//    EncryptedPkcs8,
//    RsaPrivateKey,
//    Any = -1,
//}

//public static byte[] MakePfx(string certPath, string keyPath, string exportPassword)
//{
//    using X509Certificate2 cert = new X509Certificate2(certPath);
//    byte[] keyBytes;
//    KeyFileKinds kinds;
//    ReadOnlySpan<char> keyFileText = File.ReadAllText(keyPath).AsSpan();

//    // PemEncoding.TryFind requires net5.0+
//    if (PemEncoding.TryFind(keyFileText, out PemFields pemFields))
//    {
//        keyBytes = new byte[pemFields.DecodedDataLength];

//        if (!Convert.TryFromBase64Chars(keyFileText[pemFields.Base64Data], keyBytes, out int written) ||
//            written != keyBytes.Length)
//        {
//            Debug.Fail("PemEncoding.TryFind and Convert.TryFromBase64Chars disagree on Base64 encoding");
//            throw new InvalidOperationException();
//        }

//        ReadOnlySpan<char> label = keyFileText[pemFields.Label];

//        if (label.SequenceEqual("PRIVATE KEY"))
//        {
//            kinds = KeyFileKinds.Pkcs8;
//        }
//        else if (label.SequenceEqual("ENCRYPTED PRIVATE KEY"))
//        {
//            kinds = KeyFileKinds.EncryptedPkcs8;
//        }
//        else if (label.SequenceEqual("RSA PRIVATE KEY"))
//        {
//            kinds = KeyFileKinds.RsaPrivateKey;
//        }
//        else
//        {
//            throw new NotSupportedException($"The PEM file type '{label.ToString()}' is not supported.");
//        }
//    }
//    else
//    {
//        kinds = KeyFileKinds.Any;
//        keyBytes = File.ReadAllBytes(keyPath);
//    }

//    RSA rsa = null;
//    ECDsa ecdsa = null;
//    DSA dsa = null;

//    switch (cert.GetKeyAlgorithm())
//    {
//        case "1.2.840.113549.1.1.1":
//            rsa = RSA.Create();
//            break;
//        case "1.2.840.10045.2.1":
//            ecdsa = ECDsa.Create();
//            break;
//        case "1.2.840.10040.4.1":
//            dsa = DSA.Create();
//            break;
//        default:
//            throw new NotSupportedException($"The certificate key algorithm '{cert.GetKeyAlgorithm()}' is unknown");
//    }

//    AsymmetricAlgorithm anyAlg = rsa ?? ecdsa ?? (AsymmetricAlgorithm)dsa;
//    bool loaded = false;
//    int bytesRead;

//    using (rsa)
//    using (ecdsa)
//    using (dsa)
//    {
//        if (!loaded && rsa != null && kinds.HasFlag(KeyFileKinds.RsaPrivateKey))
//        {
//            try
//            {
//                rsa.ImportRSAPrivateKey(keyBytes, out bytesRead);
//                loaded = bytesRead == keyBytes.Length;
//            }
//            catch (CryptographicException)
//            {
//            }
//        }

//        if (!loaded && kinds.HasFlag(KeyFileKinds.Pkcs8))
//        {
//            try
//            {
//                anyAlg.ImportPkcs8PrivateKey(keyBytes, out bytesRead);
//                loaded = bytesRead == keyBytes.Length;
//            }
//            catch (CryptographicException)
//            {
//            }
//        }

//        if (!loaded && kinds.HasFlag(KeyFileKinds.EncryptedPkcs8))
//        {
//            try
//            {
//                // This assumes that the private key was already exported
//                // with the same password that the PFX will be exported with.
//                // Not true? Add a parameter :).
//                anyAlg.ImportEncryptedPkcs8PrivateKey(exportPassword, keyBytes, out bytesRead);
//                loaded = bytesRead == keyBytes.Length;
//            }
//            catch (CryptographicException)
//            {
//            }
//        }

//        if (!loaded)
//        {
//            throw new InvalidOperationException("Could not load the key as any known format.");
//        }

//        X509Certificate2 withKey;

//        if (rsa != null)
//        {
//            withKey = cert.CopyWithPrivateKey(rsa);
//        }
//        else if (ecdsa != null)
//        {
//            withKey = cert.CopyWithPrivateKey(ecdsa);
//        }
//        else
//        {
//            Debug.Assert(dsa != null);
//            withKey = cert.CopyWithPrivateKey(dsa);
//        }

//        using (withKey)
//        {
//            return withKey.Export(X509ContentType.Pfx, exportPassword);
//        }
//    }
//}
//Update(2020 - 10 - 09): The previous update showed better code from .NET Core 3.1, but then also some looking ahead code from .NET 5. If the certificate file is in the PEM format (-----BEGIN CERTIFICIATE-----) and the key file is in a PEM format (BEGIN PRIVATE KEY / BEGIN RSA PRIVATE KEY / BEGIN EC PRIVATE KEY / BEGIN ENCRYPTED PRIVATE KEY) then there's an even simpler approach with .NET 5:

//using (X509Certificate2 certWithKey = X509Certificate2.CreateFromPemFile(certPath, keyPath))
//{
//    return certWithKey.Export(X509ContentType.Pfx, exportPassword);
//}
//Also available as CreateFromPem(loadedCertPem, loadedKeyPem), CreateFromEncryptedPem(loadedCertPem, loadedKeyPem, keyPassword), and CreateFromEncryptedPemFile(certPath, keyPath, keyPassword).