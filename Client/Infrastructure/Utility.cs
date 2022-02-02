namespace Infrastructure
{
	public static class Utility
	{
		static Utility()
		{
		}

		public static string ConvertByteArrayToHexString(byte[] value)
		{
			var result =
				System.BitConverter.ToString(value: value)
				.Replace("-", string.Empty)
				;

			return result;
		}

		public static string ConvertByteArrayToBase64String(byte[] value)
		{
			var result =
				System.Convert.ToBase64String
				(inArray: value, options: System.Base64FormattingOptions.None);

			return result;
		}

		public static System.DateTime Now
		{
			get
			{
				//var result =
				//	System.DateTime.Now;

				var result =
					System.DateTime.UtcNow;

				return result;
			}
		}

		public static string ConvertObjectToJson
			(object theObject, bool writeIndented = true)
		{
			var options =
				new System.Text.Json.JsonSerializerOptions
				{
					WriteIndented = writeIndented,
				};

			var result =
				System.Text.Json.JsonSerializer
				.Serialize(value: theObject, options: options);

			return result;
		}

		public static string GetSha256(string text)
		{
			var inputBytes =
				System.Text.Encoding.UTF8.GetBytes(s: text);

			var sha =
				System.Security.Cryptography.SHA256.Create();

			var outputBytes =
				sha.ComputeHash(buffer: inputBytes);

			var result =
				System.Convert.ToBase64String(inArray: outputBytes);

			sha.Dispose();
			//sha = null;

			return result;
		}
	}
}
