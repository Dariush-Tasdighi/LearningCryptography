//static void ProcessPFX()
//{
//    string pfxfile, pwd;
//    string certfile, p7file;
//    StringBuilder sbPriKey, sbPubKey;
//    int r, nCerts, i;
//    string fname, s, query;
//    string template, xmlelement, hashalg;


//    // Our PFX file (could also be .p12 = same thing)
//    pfxfile = "enid.pfx";
//    pwd = "password";   // Danger, Will Robinson!

//    // 1. First show we can read in our private key directly from the PFX file 
//    // (just like from a PKCS#8 key file)
//    // We can use this "internal" private key string to, e.g., sign data.
//    sbPriKey = Rsa.ReadPrivateKey(pfxfile, pwd);
//    Console.WriteLine("Rsa.ReadPrivateKey() returns an internal string of length {0}", sbPriKey.Length);
//    Debug.Assert(sbPriKey.Length > 0, "Failed to read private key from PFX");
//    Console.WriteLine("Pri key bits = {0}", Rsa.KeyBits(sbPriKey.ToString()));

//    // 2. Extract our X.509 certificate
//    certfile = "mycert.cer";
//    r = X509.GetCertFromPFX(certfile, pfxfile, pwd);
//    Console.WriteLine("X509.GetCertFromPFX() returns {0} (expected +ve)", r);

//    // 2a. Read in our public key from this certificate
//    sbPubKey = Rsa.ReadPublicKey(certfile);
//    Debug.Assert(sbPubKey.Length > 0, "Failed to read public key from certificate");
//    Console.WriteLine("Pub key bits = {0}", Rsa.KeyBits(sbPubKey.ToString()));

//    // 2b. Show these two keys are matched
//    Console.WriteLine("Pri key hashcode = {0:X8}", Rsa.KeyHashCode(sbPriKey));
//    Console.WriteLine("Pub key hashcode = {0:X8}", Rsa.KeyHashCode(sbPubKey));

//    // 2c. alternatively...
//    r = Rsa.KeyMatch(sbPriKey.ToString(), sbPubKey.ToString());
//    Console.WriteLine("Rsa.KeyMatch() returns {0} (expected 0)", r);
//    Debug.Assert(0 == r, "Keys do not match");

//    // 3. Extract all certificates in the chain as a single P7 file
//    // (strictly should be .p7c but .p7b works better in Windows)
//    p7file = "certs.p7b";
//    r = X509.GetP7ChainFromPFX(p7file, pfxfile, pwd);
//    Console.WriteLine("X509.GetP7ChainFromPFX() returns {0} (expected +ve)", r);
//    Debug.Assert(r > 0, "Failed to extract P7 file from PFX");

//    // 3a. Validate the chain
//    r = X509.ValidatePath(p7file);
//    Console.WriteLine("X509.ValidatePath() returns {0} (expected 0)", r);
//    Debug.Assert(r == 0, "Path validation failed");

//    // 4. Extract certs individually from the P7 file
//    //    and show details of these certificates.

//    // How many certs are there?
//    nCerts = (int)X509.GetCertFromP7Chain("", p7file, 0);
//    Console.WriteLine("nCerts = {0}", nCerts);
//    Debug.Assert(nCerts > 0, "No certs found in P7 file");

//    // Enumerate through the certs
//    for (i = 1; i <= nCerts; i++)
//    {
//        fname = "cert" + i + ".cer";
//        Console.WriteLine("FILE: {0}", fname);
//        r = X509.GetCertFromP7Chain(fname, p7file, i);
//        Debug.Assert(r > 0);

//        // 4a. Extract details from certificate in XML-DSIG-required form (LDAP, decimal)
//        query = "subjectName";
//        s = X509.QueryCert(fname, query, X509.OutputOpts.Ldap);
//        Console.WriteLine("{0}: {1}", query, s);
//        query = "issuerName";
//        s = X509.QueryCert(fname, query, X509.OutputOpts.Ldap);
//        Console.WriteLine("{0}: {1}", query, s);
//        query = "serialNumber";
//        s = X509.QueryCert(fname, query, X509.OutputOpts.Decimal);
//        Console.WriteLine("{0}: {1}", query, s);

//        // And compute its SHA-1 digest value ("thumbprint")
//        s = X509.CertThumb(fname, HashAlgorithm.Sha1);
//        Console.WriteLine("DigestValue (hex) = {0}", s);
//        // We want the base64-encoded form for XML DigestValue elements
//        Console.WriteLine("DigestValue (b64) = {0}", Cnv.ToBase64(Cnv.FromHex(s)));
//    }

//    // 5. Compose a <xades:Cert> element for a XADES file
//    Console.WriteLine("FILE: {0}", certfile);
//    template = @"<xades:Cert>
//  <xades:CertDigest>
//    <ds:DigestMethod Algorithm=""@!HASH-ALG!@"" />
//    <ds:DigestValue>@!DIGEST-VALUE!@</ds:DigestValue>
//  </xades:CertDigest>
//    <xades:IssuerSerial>
//      <ds:X509IssuerName>@!ISSUER-NAME!@</ds:X509IssuerName>
//      <ds:X509SerialNumber>@!SERIAL-NUMBER!@</ds:X509SerialNumber>
//  </xades:IssuerSerial>
//</xades:Cert>";
//    // Get element content in required form (LDAP, decimal) from cert then substitute in template.
//    xmlelement = template;
//    // 5a. Use SHA-1
//    hashalg = "http://www.w3.org/2000/09/xmldsig#sha1";
//    xmlelement = xmlelement.Replace("@!HASH-ALG!@", hashalg);
//    s = X509.CertThumb(certfile, HashAlgorithm.Sha1);
//    s = Cnv.ToBase64(Cnv.FromHex(s));
//    xmlelement = xmlelement.Replace("@!DIGEST-VALUE!@", s);
//    s = X509.QueryCert(certfile, "issuerName", X509.OutputOpts.Ldap);
//    xmlelement = xmlelement.Replace("@!ISSUER-NAME!@", s);
//    s = X509.QueryCert(certfile, "serialNumber", X509.OutputOpts.Decimal);
//    xmlelement = xmlelement.Replace("@!SERIAL-NUMBER!@", s);
//    Console.WriteLine(xmlelement);

//    // 5b. Repeat using SHA-256
//    xmlelement = template;
//    hashalg = "http://www.w3.org/2001/04/xmlenc#sha256";    // CHANGED
//    xmlelement = xmlelement.Replace("@!HASH-ALG!@", hashalg);
//    s = X509.CertThumb(certfile, HashAlgorithm.Sha256);     // CHANGED
//    s = Cnv.ToBase64(Cnv.FromHex(s));
//    xmlelement = xmlelement.Replace("@!DIGEST-VALUE!@", s);
//    s = X509.QueryCert(certfile, "issuerName", X509.OutputOpts.Ldap);
//    xmlelement = xmlelement.Replace("@!ISSUER-NAME!@", s);
//    s = X509.QueryCert(certfile, "serialNumber", X509.OutputOpts.Decimal);
//    xmlelement = xmlelement.Replace("@!SERIAL-NUMBER!@", s);
//    Console.WriteLine(xmlelement);
//}