# Apartment-Web-App
C# Course @ FER - Final project - Apartment Web App
http://mydream-krk.azurewebsites.net/


Tema projekta je jednostavna web aplikacija za apartman. U nastavku su opisane implementirane funkcionalnosti:

Mogućnosti gosta:
-	Pregled naslovne stranice
-	Mjenjanje jezika
-	Prijava

Mogućnosti registrirang korisnika:
-	Sve što i gost
-	Pristup posebnoj stranici za goste koja sadrži korisne informacije
- Ostavljanje recenzije

  - Korisnik može više puta uređivati vlastitu recenziju
  - Korisnik može dozvoliti ili zabraniti da se njegova recenzija prikazuje na naslovnoj stranici
  
Mogućnosti administratora:
-	Sve što  i registrirani korisnik
-	Pristup posebnoj stranici za upravljanje web aplikacijom
-	Upravljanje slikama
  - Dodavanje, brisanje i mjenjanje redoslijeda
  
-	Upravljanje korisničkim računima

  -	Stvaranje novog korisničkog računa za gosta 
  -	Dodjeljivanje administratorskih ovlasti  
  -	Brisanje računa
  
-	Upravljanje recenzijama

  - Stavljanje ili micanje recenzija sa naslovne stranice
  -	Brisanje recenzija

Napomene:
-	Pri pokretanju aplikacije stvara se administratorksa “uloga” (role), te glavni administrator prema postavkama u appsettings.json. Ukoliko ništa ne mjenjate, email ce biti admin@mail.com, a lozinka Admin123;
-	U appsettingsu se također može postaviti i defaultni jezik
-	Od jezika su korisniku dostupni engleski i hrvatski, njemački je također predviđen ali sakriven sa naslovne strane jer prijevod nije bio dovršen
-	Nažalost sav tekst za goste nije spreman te će se naknadno dodati

