using System.Security.Cryptography.X509Certificates;

namespace Client.Step02
{
	public static class Program
	{
		static Program()
		{
		}

		public static void Main()
		{
			// Generate asymmetric key pair
			// ECDSA: Elliptic Cureve Digital Signature Algorithm
			var ecdsa =
				System.Security.Cryptography.ECDsa.Create();

			// 139
			int size1 =
				ecdsa.GetMaxSignatureSize
				(System.Security.Cryptography.DSASignatureFormat.Rfc3279DerSequence);

			// 132
			int size2 =
				ecdsa.GetMaxSignatureSize
				(System.Security.Cryptography.DSASignatureFormat.IeeeP1363FixedFieldConcatenation);

			// 521
			int size3 = ecdsa.KeySize;

			// "ECDsa"
			string signatureAlgorithm = ecdsa.SignatureAlgorithm;

			// **************************************************
			var privateKey1 =
				ecdsa.ExportECPrivateKey();

			var privateKeyHexString =
				Infrastructure.Utility.ConvertByteArrayToBase64String(value: privateKey1);

			//System.Console.WriteLine($"{nameof(privateKey1)}: {privateKeyHexString}");
			// **************************************************

			// **************************************************
			var publicKey1 =
				ecdsa.ExportSubjectPublicKeyInfo();

			var publicKeyHexString =
				Infrastructure.Utility.ConvertByteArrayToBase64String(value: publicKey1);

			//System.Console.WriteLine($"{nameof(publicKey1)}: {publicKeyHexString}");
			// **************************************************

			string message = "Hello, World!";

			var messageBytes =
				System.Text.Encoding.UTF8.GetBytes(message);

			var signedData =
				ecdsa.SignData(data: messageBytes,
				hashAlgorithm: System.Security.Cryptography.HashAlgorithmName.SHA256);

			// **************************************************
			// **************************************************
			System.Security.Cryptography.RSAParameters parameters =
				new System.Security.Cryptography.RSAParameters()
				{
					D = null,
					P = null,
					Q = null,
					DP = null,
					DQ = null,

					Modulus = null,
					Exponent = null,
					InverseQ = null,
				};

			System.Security.Cryptography.RSA rsa =
				System.Security.Cryptography.RSA.Create(keySizeInBits: 512);

			int keySize = rsa.KeySize;

			// **************************************************
			var privateKey =
				rsa.ExportRSAPrivateKey();

			var privateKeyBase64String =
				Infrastructure.Utility.ConvertByteArrayToBase64String(value: privateKey);

			System.Console.WriteLine(System.Environment.NewLine);
			System.Console.WriteLine($"{nameof(privateKey)}");
			System.Console.WriteLine(System.Environment.NewLine);
			System.Console.WriteLine($"{privateKeyBase64String}");
			// **************************************************

			// **************************************************
			var publicKey =
				ecdsa.ExportSubjectPublicKeyInfo();

			var publicKeyBase64String =
				Infrastructure.Utility.ConvertByteArrayToBase64String(value: publicKey);

			System.Console.WriteLine(System.Environment.NewLine);
			System.Console.WriteLine($"{nameof(publicKey)}");
			System.Console.WriteLine(System.Environment.NewLine);
			System.Console.WriteLine($"{publicKeyBase64String}");
			// **************************************************
			// **************************************************
			// **************************************************

			MakeCert(rsa, certFilename: "D:\\Temp\\1.cer", "D:\\Temp\\1.pem", "D:\\Temp\\1.pfx");

			// **************************************************
			//System.Security.Cryptography.RSA
			//	newRsa = System.Security.Cryptography.RSA.Create(keySizeInBits: 512);

			////newRsa.ImportRSAPrivateKey(source: privateKey, out _);
			//newRsa.ImportSubjectPublicKeyInfo(source: publicKey, out _);
			// **************************************************

			// **************************************************
			//string password = "1234512345";

			//System.Security.Cryptography.X509Certificates.X509Certificate2
			//	certificate = new System.Security.Cryptography.X509Certificates.X509Certificate2
			//	(rawData: publicKey, password: password);

			//certificate.CopyWithPrivateKey(newRsa);

			//certificate.FriendlyName = "Mr. Dariush Tasdighi";
			//certificate.Archived = false;

			//string serialNumber =
			//	certificate.GetSerialNumberString();

			//string raw =
			//	certificate.GetRawCertDataString();

			//bool hasPrivateKey = certificate.HasPrivateKey;

			//string s = certificate.SerialNumber;

			//byte[] certData =
			//	certificate.Export
			//	(contentType: X509ContentType.Pfx, password: "MyPassword");

			//File.WriteAllBytes(@"D:\Temp\MyCert.pfx", certData);
			// **************************************************

			// CN=Patti Fuller,OU=UserAccounts,DC=corp,DC=contoso,DC=com

			//var req = new CertificateRequest("cn=foobar", ecdsa, HashAlgorithmName.SHA256);
			//var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(5));

			//// Create PFX (PKCS #12) with private key
			//File.WriteAllBytes("c:\\temp\\mycert.pfx", cert.Export(X509ContentType.Pfx, "P@55w0rd"));

			//// Create Base 64 encoded CER (public key only)
			//File.WriteAllText("c:\\temp\\mycert.cer",
			//	"-----BEGIN CERTIFICATE-----\r\n"
			//	+ Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks)
			//	+ "\r\n-----END CERTIFICATE-----");
		}

		public static void MakeCert
			(System.Security.Cryptography.RSA rsa,
			string certFilename, string keyFilename, string pfxFilename)
		{
			const string CRT_HEADER = "-----BEGIN CERTIFICATE-----\n";
			const string CRT_FOOTER = "\n-----END CERTIFICATE-----";

			const string KEY_HEADER = "-----BEGIN RSA PRIVATE KEY-----\n";
			const string KEY_FOOTER = "\n-----END RSA PRIVATE KEY-----";

			//using var rsa = System.Security.Cryptography.RSA.Create();

			System.Security.Cryptography.X509Certificates.X500DistinguishedName
				distinguishedName = new System.Security.Cryptography.X509Certificates
				.X500DistinguishedName($"CN=Patti Fuller,OU=UserAccounts,DC=corp,DC=contoso,DC=com");

			var certRequest =
				new CertificateRequest
				(distinguishedName, rsa,
				System.Security.Cryptography.HashAlgorithmName.SHA256,
				System.Security.Cryptography.RSASignaturePadding.Pkcs1);

			//var certRequest =
			//	new CertificateRequest
			//	("cn=test", rsa,
			//	System.Security.Cryptography.HashAlgorithmName.SHA256,
			//	System.Security.Cryptography.RSASignaturePadding.Pkcs1);

			// Adding SubjectAlternativeNames (SAN)
			var subjectAlternativeNames = new SubjectAlternativeNameBuilder();
			subjectAlternativeNames.AddDnsName("dariush");
			subjectAlternativeNames.AddEmailAddress("dariusht@gmail.com");
			certRequest.CertificateExtensions.Add(subjectAlternativeNames.Build());

			//var certificateExtension =
			//	new System.Security.Cryptography.X509Certificates.X509KeyUsageExtension
			//	(keyUsages: System.Security.Cryptography.X509Certificates
			//	.X509KeyUsageFlags.DigitalSignature, critical: true);

			//certRequest.CertificateExtensions.Add(certificateExtension);

			certRequest.CertificateExtensions.Add
				(new System.Security.Cryptography.X509Certificates.X509KeyUsageExtension
				(System.Security.Cryptography.X509Certificates.X509KeyUsageFlags.DataEncipherment |
				System.Security.Cryptography.X509Certificates.X509KeyUsageFlags.KeyEncipherment |
				System.Security.Cryptography.X509Certificates.X509KeyUsageFlags.DigitalSignature, false));

			certRequest.CertificateExtensions.Add
				(new System.Security.Cryptography.X509Certificates.X509EnhancedKeyUsageExtension
				(new System.Security.Cryptography.OidCollection
				{
					new System.Security.Cryptography.Oid("1.3.6.1.5.5.7.3.1")
				}, false));

			var certificate =
				certRequest.CreateSelfSigned
				(System.DateTimeOffset.Now, System.DateTimeOffset.Now.AddSeconds(5));

			// export the private key
			var privateKey =
				System.Convert.ToBase64String
				(rsa.ExportRSAPrivateKey(), System.Base64FormattingOptions.InsertLineBreaks);

			System.IO.File.WriteAllText
				(keyFilename, KEY_HEADER + privateKey + KEY_FOOTER);

			// Export the certificate
			var exportData =
				certificate.Export(X509ContentType.Cert);

			var crt =
				System.Convert.ToBase64String
				(exportData, System.Base64FormattingOptions.InsertLineBreaks);

			System.IO.File.WriteAllText
				(certFilename, CRT_HEADER + crt + CRT_FOOTER);

			// Export the PFX
			var pfxData =
				certificate.Export(X509ContentType.Pfx, password: "12345");

			System.IO.File.WriteAllBytes(pfxFilename, pfxData);
		}
	}
}
