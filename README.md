<img src="https://github.com/jil1710/readmedemo/assets/125335932/123ae210-4eb3-466b-8395-348f41546036" width="250px" height="150px"/>

# Authentication And Authorization

- Authentication is the process of determining a user's identity.
- Authorization is the process of determining whether a user has access to a resource.
- In ASP.NET Core, authentication is handled by the authentication service, IAuthenticationService, which is used by authentication middleware. The authentication service uses registered authentication handlers to complete authentication-related actions. Examples of authentication-related actions include:
    - Authenticating a user.
    - Responding when an unauthenticated user tries to access a restricted resource.
 
## Cookie Authentication

- Cookie authentication in ASP.NET Core web application is the popular choice for developers to implement authentication in most customer-facing web applications and is also easy to implement in ASP.NET Core as it is provided out of the box without the need to reference any additional NuGet packages.
- ASP.NET Core provides a cookie authentication mechanism which on login serializes the user details in form of claims into an encrypted cookie and then sends this cookie back to the server on subsequent requests which gets validated to recreate the user object from claims and sets this user object in the HttpContext so that it is available & is valid only for that request.

- **How does cookie authentication in ASP.NET Core work?**

  ![image](https://github.com/jil1710/readmedemo/assets/125335932/b219f13f-472b-4998-9b34-f726faeede42)

  - User requests URL from the server
  - Based on the access set server will either return the page if it’s a public page or will return the login page if its a secured page i.e. asking the user to login to access the secured page
  - User will perform login action & credentials will be sent to the server for validation
  - If provided credentials are valid then the server will set an authentication cookie in response.
  - Now all further requests from the user will carry this cookie in the header so that on each request server knows that the user is already authenticated & also user details can be read from cookie to identity request if from which user.

- **Let's Implement Cookie Authentication:**

  - First Create ASP.Net Core Project And Add LoginModel and User model for role base authorization:

    ![image](https://github.com/jil1710/readmedemo/assets/125335932/ea52c3c1-02a8-40d9-b6ed-ae6c0073dadd)

    ![image](https://github.com/jil1710/readmedemo/assets/125335932/c52c93b8-70e0-414d-8811-1c60cddd7956)

  - Now seed some user into List<User> or you can use database or identity framework to validate or storing of user. Here for i am using list of User static data as storage.

    ![image](https://github.com/jil1710/readmedemo/assets/125335932/a4a85237-21ab-40f6-920d-6f1f2579237d)

  - After that Cookie Authentication scheme and Authorization policy into program.cs file

    ![image](https://github.com/jil1710/readmedemo/assets/125335932/98fef953-8926-49d2-8a9a-c50a968dc116)

  - Add This two middleware in same order as below :

    ![image](https://github.com/jil1710/readmedemo/assets/125335932/991e8ad1-1881-4a46-81b4-a52af98c016a)

  - Add login api that validate the user and authenticate the user then create the session by storing the cookie.

    ![image](https://github.com/jil1710/readmedemo/assets/125335932/0dc20e57-bed6-46e2-bcae-15c627b4de7a)

    ![image](https://github.com/jil1710/readmedemo/assets/125335932/7bb81bd9-39df-4454-baf1-432102f682a5)


  - Then Create secret route that is only access by `Administrition` role and authenticated user.

    ![image](https://github.com/jil1710/readmedemo/assets/125335932/823ea762-c6f5-4a8d-8aba-0a3d1938bd5b)

  Click here to see the example : [Cookie Authentication](https://github.com/jil1710/Authentication-Authorization/tree/master/CookieAuthentication)

# JWT and Multiple Scheme Authentication

- JSON Web Token is an open standard (RFC 7519) that defines a safe, compact, and self-contained, secured way for transmission of information between a sender and a receiver through a URL, a POST parameter, or inside the HTTP Header. It should be noted that the information to be transmitted securely between two parties is represented in JSON format and it is cryptographically signed to verify its authenticity. JWT is typically used for implementing authentication and authorization in Web applications. Because JWT is a standard, all JWTs are tokens but the reverse is not true. You can work with JSON Web Tokens in .NET, Python, Node.js, Java, PHP, Ruby, Go, JavaScript, etc.

  ![image](https://github.com/jil1710/readmedemo/assets/125335932/830f1ed7-c025-48b6-b5fe-760709c2dc50)

- JWT is represented as a combination of three base64url encoded parts concatenated with period ('.') characters and comprises the following three sections:

  - **Header**
  - **Payload**
  - **Signature**

- **Header Section**

    - This section provides metadata about the type of data and the algorithm to be used to encrypt the data that is to be transferred. The JWT header comprises three sections - these include: the metadata for the token, the type of signature, and the encryption algorithm. It comprises two properties, i.e., “alg” and “typ”. Although the former relates to the cryptography algorithm used, i.e., HS256 in this case, the latter is used to specify the type of the token, i.e., JWT in this case.
      ```json
      {
          "typ": "JWT",
          "alg": "HS256"
      }
      ```
- **Payload**
  
    - The payload represents the actual information in JSON format that is to be transmitted over the wire. The code snippet given below illustrates a simple payload.
      ```json
        {
          "sub": "1234567890",
          "name": "Joydip Kanjilal",
          "admin": true,
          "claims : { ... }
          "jti": "cdafc246-109d-4ac9-9aa1-eb689fad9357",
          "iat": 1611497332,
          "exp": 1611500932
        }
      ```
    - iss: This represents the issuer of the token.
    - sub: This is the subject of the token.
    - aud: This represents the audience of the token.
    - exp: This is used to define token expiration.
    - nbf: This is used to specify the time before which the token must not be processed.
    - iat: This represents the time when the token was issued.
    - jti: This represents a unique identifier for the token.

- **Signature**

    - The signature adheres to the JSON Web Signature (JWS) specification and is used to verify the integrity of the data transferred over the wire. It comprises a hash of the header, the payload, and the secret, and is used to ensure that the message was not changed while being transmitted. The final signed token is created by adhering to the JSON Web Signature (JWS) specification. The encoded JWT header and as well as the encoded JWT payload is combined and then it's signed using a strong encryption algorithm such as HMAC SHA 256.
 
- **Let's Implement JWT Authentication and How to use multiple scheme in project**

  - First of all create ASP.Net Core Application and install below mention nueget packages :

    ```csharp
        Microsoft.AspNetCore.Authentication
    ```
    ```csharp
        Microsoft.AspNetCore.Authentication.JwtBearer
    ```

  - After that add LoginModel and User model for role base authorization:

    ![image](https://github.com/jil1710/readmedemo/assets/125335932/2ae59ebf-4bc2-4afc-bf09-6be3d43bdee5)
    ![image](https://github.com/jil1710/readmedemo/assets/125335932/d930936a-2416-432e-a128-ebb798d1a8f1)

  - Now seed some user into List<User> or you can use database or identity framework to validate or storing of user. Here for i am using list of User static data as storage.

    ![image](https://github.com/jil1710/readmedemo/assets/125335932/a4a85237-21ab-40f6-920d-6f1f2579237d)

  - Configuring JWT in the AppSettings File ( appsettings.json ) by creating the section of JWT as shown in below:

    ```json
        "JWT":
        {
            "Key": "qwertyuiopLKJHGFDSAzxcvbnm",
            "Issuer": "https://localhost:7202",
            "Audience": "https://localhost:7202"
        }
    ```
    
  - Configure Authentication with Bearer and JWT in addition also configure cookie authentication because in parallel we understant how to use multiple authentication scheme.

    ![image](https://github.com/jil1710/readmedemo/assets/125335932/93674099-c059-4dde-a4a1-41623235d923)

  - Add policy for both the scheme

    ![image](https://github.com/jil1710/readmedemo/assets/125335932/4112df1b-d38b-43ce-92c7-73bd5d42f7ce)

  - Add This two middleware in same order as below :

    ![image](https://github.com/jil1710/readmedemo/assets/125335932/991e8ad1-1881-4a46-81b4-a52af98c016a)

  - Add login route that use jwt scheme and return access token

    ![image](https://github.com/jil1710/readmedemo/assets/125335932/ef65ef04-55ed-41af-a192-954de25128ba)

  - Also create secret route that is only access by jwt access token schem by add jwt policy which we created above.

    ![image](https://github.com/jil1710/readmedemo/assets/125335932/62262c89-4678-4e96-b81d-06e3a56dc240)

  - Similarlly, add login route that use cookie authentication scheme and authenticate user.

    ![image](https://github.com/jil1710/readmedemo/assets/125335932/664b87f5-f566-4b15-86c2-4134e2785f52)

  - Also create secret route that is only access by cookie schem by add cookie policy which we created above.

    ![image](https://github.com/jil1710/readmedemo/assets/125335932/d6bb7cba-7866-4998-ba4e-7f5c4410b036)

  - Click here to see the example : [Jwt and Multiple Atuhentication scheme](https://github.com/jil1710/Authentication-Authorization/tree/master/MultipleAuthScheme)


# Customs Authorize attribute

- we can also create our own custom authoriza attribue.

- Let's create `RoleRequired` attribute. So to creat custom authoriza attribute then implement the `TypeFilterAttribute` and pass the type of Iauthorization filter so below :

  ![image](https://github.com/jil1710/readmedemo/assets/125335932/df8575e5-7195-4e32-a9e7-dee4c6bfffb9)

- Now use `RoleRequired` Attribute if user has claim that we are passing in constructor then they get access  to the resource.

  ![image](https://github.com/jil1710/readmedemo/assets/125335932/7ce01381-2ad5-4d38-b1f9-6548a1b6f89c)

- Click here to see the example : [Custom Authorize Attribute](https://github.com/jil1710/Authentication-Authorization/tree/master/CustomAuthorizeAttribute)

# Using Identity Server

- Identity Server4 is an open-source authentication provider with OpenID connect and OAuth2.0 framework for ASP.NET Core. It acts as a centralized authentication provider or security token server (STS). It will be ideal to go through layers when you have multiple API/microservices applications and you should have single security token server to handle the authentication so that you really don’t have to define the Authentication in each and every application.   

- Identity server4 is a simple and straightforward STS. The user uses the clients (ASP.NET MVC or angular or react application and so on) to access the data, these users are authenticated by Identity Server to use the client. After successful authentication, the Identity server will send a token to client. Then client should use this token to access the data from the APIs. 

- The identityServer4 is the implementation of OpenID Connect and OAuth 2.0 to secure mobile, native and web applications. It acts as a middleware and a single source where you can integrate with multiple application to frame the authentication layer to secure your applications.

- **Let's Implement IdentityServer4**

  - Create ASP.NET Core Project with IdentityServer 4 and Install below nuget package.
    ```csharp
        Install-Package IdentityServer4
    ```

  - **Adding in-memory configuration**
 
    **1. Identity Resource :** The data like UserId, phone number, email which has something unique to a particular identity/user are the Identity Resource. Add the following code to IdentityConfigration class.

    ![image](https://github.com/jil1710/readmedemo/assets/125335932/8e9c0eb6-2f0e-4f46-aa15-437da215f295)

    **2. Add API Resource :** Let’s define the API Resource with Scopes and API Secrets. Ensure to hash this secret code. This secret hashed code will be saved internal within IdentityServer4..

    ![image](https://github.com/jil1710/readmedemo/assets/125335932/f64b53b8-75e6-475a-a55d-05302c5e0b1e)

    **3. Add Clients :** Let’s define the clients by giving proper clientId and name. We also have to define who will be granted access to our protected resource. In our case it is GetProduct. Add following code to IdentityConfigration class.

    ![image](https://github.com/jil1710/readmedemo/assets/125335932/5eb0dbf4-dcb0-4327-a07d-9456bae2153d)

    **4. Registering IdentityServer4 in ASP.NET Core :**

    ![image](https://github.com/jil1710/readmedemo/assets/125335932/c6236ad2-b86f-48c3-bb18-2b3312929bc4)

    **5. Add Identity Server Middle**

    ![image](https://github.com/jil1710/readmedemo/assets/125335932/514af3dc-189f-4695-a390-ed5b4c5bde0e)


- **Now Setup the Client Side and redirect to authentication page of IdentityServer4.**

  - **Install below nuget package**
    ```csharp
        Microsoft.AspNetCore.Authentication.OpenIdConnect
    ```

  - After that register OpenIdConnect in program.cs file

    ![image](https://github.com/jil1710/readmedemo/assets/125335932/bcd29e8f-a2bc-437d-96d7-481d48a8ef71)

    - Aurthority : our identity server url.
    - client id : add client which we created in identityserver configuration.
    - secret key : add secret key which we created in identityserver configuration.
    - save token : to save the access token in client side cookie.
   
  - After that create aurthorize route and try to hit that route it will redirect to identity server login after authenticated and in back channel it will generating the token and redirect to that secret authorize page.

    ![image](https://github.com/jil1710/readmedemo/assets/125335932/872ae9cc-ffa6-42c1-853e-a3fc7e69eefd)

  - Output :

    - See the below image and also the url when we hit secret tab then it will redirect to identity server if we are already authenticated then it show the authorize page.
      
        ![image](https://github.com/jil1710/readmedemo/assets/125335932/08a193cb-39d9-4c10-85d8-a2a84058fdce)
 
    - After entering the valid credential and hit the login button then it will authenticate successfull and then redirect back to secret authoriza page.

        ![image](https://github.com/jil1710/readmedemo/assets/125335932/c99aab6a-bb37-4bda-9cc2-df33b8f7e05d)

        ![image](https://github.com/jil1710/readmedemo/assets/125335932/7d9a1c7e-9a6f-4e8f-a863-6502b366ddad)

- Click here to see the example : [Client Side Code](https://github.com/jil1710/Authentication-Authorization/tree/master/Client_MVC)

- Click here to see the example : [Identity Server Code](https://github.com/jil1710/Authentication-Authorization/tree/master/OpenIdServer)
 











    
















  