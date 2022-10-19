using APIProject;
using APIProject.ApiImpl;
using Newtonsoft.Json;


var apiKey = Config.APIKEY_STAG;
var apiSecret = Config.APISECRET_STAG;
var apiDomain = Config.DOMAIN_STAG;

//var apiKey = Config.APIKEY_DEV;
//var apiSecret = Config.APISECRET_DEV;
//var apiDomain = Config.DOMAIN_DEV;

// get accessToken
var accessToken = new GetAccessToken(apiDomain, apiKey, apiSecret).execute();

// get account info
new GetAccountInfo(apiDomain, accessToken, apiSecret).execute();

// upload document
var contractNum = new UploadDocument(apiDomain, accessToken, apiSecret, @"C:\Documents\temp\testDoc\demo.pdf").execute();

// get manual sign link
var res = new ManualSign(apiDomain, accessToken, apiSecret, contractNum).execute();

// auto sign document
//new AutoSign(apiDomain, accessToken, apiSecret, contractNum).execute();