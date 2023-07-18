## Expected files

### Database.json

```
{
  "ConnectionStrings": {
    "Local": " ... ",
    "Remote": " ...",
    "Production": " ..."
  }
}
```

### Email.json

```
{
  "Features": {
    "Email": {
      "Accounts": {
        "PasswordReset": {
          "Address": "Sender",
          "Address": "foo@bar.none",
          "Password": "host.domain.tld",
          "SmtpHost": "host.domain.tld",
          "SmtpPort": 1337
        },
        "EmailVerify": {
          "Same": "...",
        }
      }
    }
  }
}
```
