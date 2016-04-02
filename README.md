# LibAllDebrid

C# api wrapper to alldebrid.com

#### Login
```c#
string login = "username";
string password = "password";
AllDebrid debridr = new AllDebrid(login, password);
```

#### Use saved cookie
```c#
string cookieSaved = "cookie-value";
AllDebrid debridr = new AllDebrid(cookie=cookieSaved);
````

#### Unleash URL
```c#
string link = "http://uptobox.com/abcde";
Link link = debridr.getUrlDebride(link);

Console.WriteLine(link.link)
```

#### Get days remaining before subcription expiration
```c#
string days = debridr.daysLeft;
```
  


