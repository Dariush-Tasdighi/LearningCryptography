//namespace Client.Step01
//{
//	public static class Program
//	{
//		static Program()
//		{
//		}

//		public static void Main()
//		{
//			System.Console.WriteLine("Step (1)");

//			// **************************************************
//			string secp256r1Oid = "1.2.840.10045.3.1.7";
//			// oid for prime256v1(7) other identifier: secp256r1

//			string subjectName = "Self-Signed-Cert-Example";

//			var ecdsa =
//				System.Security.Cryptography.ECDsa.Create
//				(System.Security.Cryptography.ECCurve.CreateFromValue(oidValue: secp256r1Oid));

//			var certRequest =
//				new System.Security.Cryptography.X509Certificates
//				.CertificateRequest($"CN={subjectName}", ecdsa,
//				System.Security.Cryptography.HashAlgorithmName.SHA256);

//			var certificateExtension =
//				new System.Security.Cryptography.X509Certificates.X509KeyUsageExtension
//				(keyUsages: System.Security.Cryptography.X509Certificates
//				.X509KeyUsageFlags.DigitalSignature, critical: true);

//			// add extensions to the request (just as an example)
//			// add keyUsage
//			certRequest.CertificateExtensions.Add(item: certificateExtension);

//			System.Security.Cryptography.X509Certificates.X509Certificate2
//				generatedCert = certRequest.CreateSelfSigned
//				(notBefore: DateTimeOffset.Now.AddDays(-1), notAfter: DateTimeOffset.Now.AddYears(10));
//			// generate the cert and sign!

//			System.Security.Cryptography.X509Certificates.X509Certificate2
//				pfxGeneratedCert =
//				new System.Security.Cryptography.X509Certificates.X509Certificate2
//				(generatedCert.Export(System.Security.Cryptography.X509Certificates.X509ContentType.Pfx));
//			// has to be turned into pfx or Windows at least throws a security credentials
//			// not found during sslStream.connectAsClient or HttpClient request...
//			// **************************************************

//			// **************************************************
//			// **************************************************
//		}

//		public static System.Security.Cryptography.X509Certificates.X509Certificate2
//			buildSelfSignedServerCertificate(string certificateName)
//		{
//			System.Security.Cryptography.X509Certificates.SubjectAlternativeNameBuilder
//				sanBuilder = new System.Security.Cryptography.X509Certificates.SubjectAlternativeNameBuilder();

//			sanBuilder.AddIpAddress(System.Net.IPAddress.Loopback);
//			sanBuilder.AddIpAddress(System.Net.IPAddress.IPv6Loopback);
//			sanBuilder.AddDnsName("localhost");
//			sanBuilder.AddDnsName(Environment.MachineName);

//			System.Security.Cryptography.X509Certificates.X500DistinguishedName
//				distinguishedName = new System.Security.Cryptography.X509Certificates
//				.X500DistinguishedName($"CN={certificateName}");

//			using (System.Security.Cryptography.RSA rsa = System.Security.Cryptography.RSA.Create(2048))
//			{
//				var request =
//					new System.Security.Cryptography.X509Certificates
//					.CertificateRequest
//					(distinguishedName, rsa, System.Security.Cryptography.HashAlgorithmName.SHA256,
//					System.Security.Cryptography.RSASignaturePadding.Pkcs1);

//				request.CertificateExtensions.Add
//					(new System.Security.Cryptography.X509Certificates.X509KeyUsageExtension
//					(System.Security.Cryptography.X509Certificates.X509KeyUsageFlags.DataEncipherment |
//					System.Security.Cryptography.X509Certificates.X509KeyUsageFlags.KeyEncipherment |
//					System.Security.Cryptography.X509Certificates.X509KeyUsageFlags.DigitalSignature, false));

//				request.CertificateExtensions.Add
//					(new System.Security.Cryptography.X509Certificates.X509EnhancedKeyUsageExtension
//					(new System.Security.Cryptography.OidCollection
//					{
//						new System.Security.Cryptography.Oid("1.3.6.1.5.5.7.3.1")
//					}, false));

//				request.CertificateExtensions.Add(sanBuilder.Build());

//				var certificate =
//					request.CreateSelfSigned
//					(new DateTimeOffset(DateTime.UtcNow.AddDays(-1)),
//					new DateTimeOffset(DateTime.UtcNow.AddDays(3650)));

//				certificate.FriendlyName = certificateName;

//				return
//					new System.Security.Cryptography.X509Certificates.X509Certificate2
//					(certificate.Export(System.Security.Cryptography.X509Certificates.X509ContentType.Pfx,
//					"WeNeedASaf3rPassword"),
//					"WeNeedASaf3rPassword",
//					System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.MachineKeySet);
//			}
//		}

//		public static System.Security.Cryptography.X509Certificates.X509Certificate2
//			CreateCertificate()
//		{
//			using (System.Security.Cryptography.RSA parent = System.Security.Cryptography.RSA.Create(4096))
//			using (System.Security.Cryptography.RSA rsa = System.Security.Cryptography.RSA.Create(2048))
//			{
//				System.Security.Cryptography.X509Certificates.CertificateRequest parentReq =
//					new System.Security.Cryptography.X509Certificates.CertificateRequest
//					("CN=Experimental Issuing Authority",
//					parent,
//					System.Security.Cryptography.HashAlgorithmName.SHA256,
//					System.Security.Cryptography.RSASignaturePadding.Pkcs1);

//				parentReq.CertificateExtensions.Add(
//					new System.Security.Cryptography.X509Certificates
//					.X509BasicConstraintsExtension(true, false, 0, true));

//				parentReq.CertificateExtensions.Add(
//					new System.Security.Cryptography.X509Certificates
//					.X509SubjectKeyIdentifierExtension(parentReq.PublicKey, false));

//				using (System.Security.Cryptography.X509Certificates.X509Certificate2 parentCert
//					= parentReq.CreateSelfSigned(
//					DateTimeOffset.UtcNow.AddDays(-45),
//					DateTimeOffset.UtcNow.AddDays(365)))
//				{
//					System.Security.Cryptography.X509Certificates.CertificateRequest req =
//						new System.Security.Cryptography.X509Certificates.CertificateRequest(
//						"CN=Valid-Looking Timestamp Authority",
//						rsa,
//						System.Security.Cryptography.HashAlgorithmName.SHA256,
//						System.Security.Cryptography.RSASignaturePadding.Pkcs1);

//					req.CertificateExtensions.Add(
//						new System.Security.Cryptography.X509Certificates
//						.X509BasicConstraintsExtension(false, false, 0, false));

//					req.CertificateExtensions.Add(
//						new System.Security.Cryptography.X509Certificates.X509KeyUsageExtension(
//							System.Security.Cryptography.X509Certificates.X509KeyUsageFlags.DigitalSignature |
//							System.Security.Cryptography.X509Certificates.X509KeyUsageFlags.NonRepudiation,
//							false));

//					req.CertificateExtensions.Add(
//						new System.Security.Cryptography.X509Certificates.X509EnhancedKeyUsageExtension(
//							new System.Security.Cryptography.OidCollection
//							{
//								new System.Security.Cryptography.Oid("1.3.6.1.5.5.7.3.8")
//							},
//							true));

//					req.CertificateExtensions.Add(
//						new System.Security.Cryptography.X509Certificates
//						.X509SubjectKeyIdentifierExtension(req.PublicKey, false));

//					using (System.Security.Cryptography.X509Certificates.X509Certificate2 cert = req.Create(
//						parentCert,
//						DateTimeOffset.UtcNow.AddDays(-1),
//						DateTimeOffset.UtcNow.AddDays(90),
//						new byte[] { 1, 2, 3, 4 }))
//					{
//						// Do something with these certs, like export them to PFX,
//						// or add them to an X509Store, or whatever.
//					}
//				}
//			}

//			return null;
//		}

//		public static System.Security.Cryptography.X509Certificates.X509Certificate2
//			AttachingAPrivateKeyToACertificate(System.Security.Cryptography.ECDiffieHellman privateKey)
//		{
//			using (System.Security.Cryptography.X509Certificates.X509Certificate2
//				pubOnly = new System.Security.Cryptography.X509Certificates.X509Certificate2("myCert.crt"))

//			using (System.Security.Cryptography.X509Certificates.X509Certificate2
//				pubPrivEphemeral = pubOnly.CopyWithPrivateKey(privateKey: privateKey))
//			{
//				// Export as PFX and re-import if you want "normal PFX private key lifetime"
//				// (this step is currently required for SslStream, but not for most other things
//				// using certificates)
//				return new System.Security.Cryptography.X509Certificates
//					.X509Certificate2(pubPrivEphemeral
//					.Export(System.Security.Cryptography.X509Certificates.X509ContentType.Pfx));
//			}
//		}

//		public static System.Security.Cryptography.X509Certificates.X509Certificate2 CreateAMinimalDummyX509Certificate2()
//		{
//			byte[] embeddedCert;

//			System.Reflection.Assembly thisAssembly =
//				System.Reflection.Assembly.GetAssembly(typeof(MyType));

//			using (Stream certStream = thisAssembly.GetManifestResourceStream("YourProjectName.localhost.pfx"))
//			{
//				embeddedCert =
//					new byte[certStream.Length];

//				certStream.Read(embeddedCert, 0, (int)certStream.Length);
//			}

//			var signingCert =
//				new System.Security.Cryptography.X509Certificates
//				.X509Certificate2(rawData: embeddedCert, password: "password");
//		}

//		static System.Security.Cryptography.X509Certificates.X509Certificate2? GetRandomCertificate()
//		{
//			System.Security.Cryptography.X509Certificates.X509Store
//				st = new System.Security.Cryptography.X509Certificates.X509Store
//				(System.Security.Cryptography.X509Certificates.StoreName.My,
//				System.Security.Cryptography.X509Certificates.StoreLocation.LocalMachine);

//			st.Open(System.Security.Cryptography.X509Certificates.OpenFlags.ReadOnly);

//			try
//			{
//				var certCollection = st.Certificates;

//				if (certCollection.Count == 0)
//				{
//					return null;
//				}

//				return certCollection[0];
//			}
//			finally
//			{
//				st.Close();
//			}
//		}

//		public static X509Certificate2 GenerateSelfSignedCertificate(string subjectName, string issuerName, AsymmetricKeyParameter issuerPrivKey, int keyStrength = 2048)
//		{
//			// Generating Random Numbers
//			var randomGenerator = new CryptoApiRandomGenerator();
//			var random = new SecureRandom(randomGenerator);

//			// The Certificate Generator
//			var certificateGenerator = new X509V3CertificateGenerator();

//			// Serial Number
//			var serialNumber = BigIntegers.CreateRandomInRange(BigInteger.One, BigInteger.ValueOf(Int64.MaxValue), random);
//			certificateGenerator.SetSerialNumber(serialNumber);

//			// Signature Algorithm
//			const string signatureAlgorithm = "SHA256WithRSA";
//			certificateGenerator.SetSignatureAlgorithm(signatureAlgorithm);

//			// Issuer and Subject Name
//			var subjectDN = new X509Name(subjectName);
//			var issuerDN = new X509Name(issuerName);
//			certificateGenerator.SetIssuerDN(issuerDN);
//			certificateGenerator.SetSubjectDN(subjectDN);

//			// Valid For
//			var notBefore = DateTime.UtcNow.Date;
//			var notAfter = notBefore.AddYears(2);

//			certificateGenerator.SetNotBefore(notBefore);
//			certificateGenerator.SetNotAfter(notAfter);

//			// Subject Public Key
//			AsymmetricCipherKeyPair subjectKeyPair;
//			var keyGenerationParameters = new KeyGenerationParameters(random, keyStrength);
//			var keyPairGenerator = new RsaKeyPairGenerator();
//			keyPairGenerator.Init(keyGenerationParameters);
//			subjectKeyPair = keyPairGenerator.GenerateKeyPair();

//			certificateGenerator.SetPublicKey(subjectKeyPair.Public);

//			// Generating the Certificate
//			var issuerKeyPair = subjectKeyPair;

//			// Selfsign certificate
//			var certificate = certificateGenerator.Generate(issuerPrivKey, random);

//			// Corresponding private key
//			PrivateKeyInfo info = PrivateKeyInfoFactory.CreatePrivateKeyInfo(subjectKeyPair.Private);

//			// Merge into X509Certificate2
//			var x509 = new System.Security.Cryptography.X509Certificates.X509Certificate2(certificate.GetEncoded());

//			var seq = (Asn1Sequence)Asn1Object.FromByteArray(info.PrivateKey.GetDerEncoded());
//			if (seq.Count != 9)
//				throw new PemException("malformed sequence in RSA private key");

//			var rsa = new RsaPrivateKeyStructure(seq);
//			RsaPrivateCrtKeyParameters rsaparams = new RsaPrivateCrtKeyParameters(
//				rsa.Modulus, rsa.PublicExponent, rsa.PrivateExponent, rsa.Prime1, rsa.Prime2, rsa.Exponent1, rsa.Exponent2, rsa.Coefficient);

//			x509.PrivateKey = DotNetUtilities.ToRSA(rsaparams);
//			return x509;
//		}

//		public static AsymmetricKeyParameter GenerateCACertificate(string subjectName, int keyStrength = 2048)
//		{
//			// Generating Random Numbers
//			var randomGenerator = new CryptoApiRandomGenerator();
//			var random = new SecureRandom(randomGenerator);

//			// The Certificate Generator
//			var certificateGenerator = new X509V3CertificateGenerator();

//			// Serial Number
//			var serialNumber = BigIntegers.CreateRandomInRange(BigInteger.One, BigInteger.ValueOf(Int64.MaxValue), random);
//			certificateGenerator.SetSerialNumber(serialNumber);

//			// Signature Algorithm
//			const string signatureAlgorithm = "SHA256WithRSA";
//			certificateGenerator.SetSignatureAlgorithm(signatureAlgorithm);

//			// Issuer and Subject Name
//			var subjectDN = new X509Name(subjectName);
//			var issuerDN = subjectDN;
//			certificateGenerator.SetIssuerDN(issuerDN);
//			certificateGenerator.SetSubjectDN(subjectDN);

//			// Valid For
//			var notBefore = DateTime.UtcNow.Date;
//			var notAfter = notBefore.AddYears(2);

//			certificateGenerator.SetNotBefore(notBefore);
//			certificateGenerator.SetNotAfter(notAfter);

//			// Subject Public Key
//			AsymmetricCipherKeyPair subjectKeyPair;
//			var keyGenerationParameters = new KeyGenerationParameters(random, keyStrength);
//			var keyPairGenerator = new RsaKeyPairGenerator();
//			keyPairGenerator.Init(keyGenerationParameters);
//			subjectKeyPair = keyPairGenerator.GenerateKeyPair();

//			certificateGenerator.SetPublicKey(subjectKeyPair.Public);

//			// Generating the Certificate
//			var issuerKeyPair = subjectKeyPair;

//			// Selfsign certificate
//			var certificate = certificateGenerator.Generate(issuerKeyPair.Private, random);
//			var x509 = new System.Security.Cryptography.X509Certificates.X509Certificate2(certificate.GetEncoded());

//			// Add CA certificate to Root store
//			addCertToStore(cert, StoreName.Root, StoreLocation.CurrentUser);

//			return issuerKeyPair.Private;
//		}

//		public static bool addCertToStore(System.Security.Cryptography.X509Certificates.X509Certificate2 cert, System.Security.Cryptography.X509Certificates.StoreName st, System.Security.Cryptography.X509Certificates.StoreLocation sl)
//		{
//			bool bRet = false;

//			try
//			{
//				X509Store store = new X509Store(st, sl);
//				store.Open(OpenFlags.ReadWrite);
//				store.Add(cert);

//				store.Close();
//			}
//			catch
//			{

//			}

//			return bRet;
//		}

//		//var caPrivKey = GenerateCACertificate("CN=root ca");
//		//var cert = GenerateSelfSignedCertificate("CN=127.0.01", "CN=root ca", caPrivKey);
//		//addCertToStore(cert, StoreName.My, StoreLocation.CurrentUser);
//	}
//}
