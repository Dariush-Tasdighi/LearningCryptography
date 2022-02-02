namespace Client.Step02
{
	/// <summary>
	/// https://www.scottbrady91.com/c-sharp/ecdsa-key-loading
	/// https://www.scottbrady91.com/c-sharp/jwt-signing-using-ecdsa-in-dotnet-core
	/// https://blog.todotnet.com/2018/02/public-private-keys-and-signing/

	/// https://kjur.github.io/jsrsasign/sample/sample-ecdsa.html
	/// https://www.freecodecamp.org/news/how-to-generate-your-very-own-bitcoin-private-key-7ad0f4936e6c
	/// https://en.wikipedia.org/wiki/Elliptic_Curve_Digital_Signature_Algorithm
	/// </summary>
	public static class EcdsaUtility
	{
		static EcdsaUtility()
		{
			Curve =
				System.Security.Cryptography.ECCurve.NamedCurves.nistP256;
		}

		public static System.Security.Cryptography.ECCurve Curve { get; }

		public static void GenerateKey
			(out byte[]? privateKey, out byte[]? publicKey)
		{
			System.Security.Cryptography.ECParameters? param;

			using (var dsa = System.Security.Cryptography.ECDsa.Create(curve: Curve))
			{
				param =
					dsa.ExportParameters(includePrivateParameters: true);
			}

			privateKey = param.Value.D;
			//publicKey = param.Value.Q.;
			publicKey = null;
		}

		//public static byte[] Sign
		//	(byte[] hash, byte[] privateKey, byte[] publicKey)
		//{
		//	var param =
		//		new System.Security.Cryptography.ECParameters
		//		{
		//			Curve = Curve,
		//			D = privateKey,
		//			Q = ToEcPoint(publicKey),
		//		};

		//	using (var dsa = System.Security.Cryptography.ECDsa.Create(param))
		//	{
		//		return dsa.SignHash(hash);
		//	}
		//}

		//public static bool Verify
		//	(byte[] hash, byte[] signature, byte[] publicKey)
		//{
		//	var param =
		//		new System.Security.Cryptography.ECParameters
		//		{
		//			Curve = Curve,
		//			Q = ToEcPoint(publicKey),
		//		};

		//	using (var dsa = System.Security.Cryptography.ECDsa.Create(param))
		//	{
		//		return dsa.VerifyHash(hash, signature);
		//	}
		//}

		public static bool TestKey
			(byte[] privateKey, byte[] publicKey)
		{
			byte[] testHash;

			using (var sha = System.Security.Cryptography.SHA256.Create())
			{
				testHash =
					sha.ComputeHash(buffer: new byte[0]);
			}

			return false;

			//try
			//{
			//	var signature =
			//		Sign(testHash, privateKey, publicKey);

			//	return Verify(testHash, signature, publicKey);
			//}
			//catch
			//{
			//	return false;
			//}
		}

		//private static byte[] ToBytes(System.Security.Cryptography.ECPoint point)
		//{
		//	return MessagePackSerializer.Serialize(
		//		new FormattableEcPoint { X = point.X, Y = point.Y });
		//}

		//private static System.Security.Cryptography.ECPoint ToEcPoint(byte[] bytes)
		//{
		//	var pt =
		//		MessagePackSerializer
		//		.Deserialize<FormattableEcPoint>(bytes);

		//	return new System.Security.Cryptography.ECPoint { X = pt.X, Y = pt.Y };
		//}

		//[MessagePackObject]
		//public class FormattableEcPoint
		//{
		//	[Key(0)]
		//	public virtual byte[] X { get; set; }

		//	[Key(1)]
		//	public virtual byte[] Y { get; set; }
		//}
	}
}
