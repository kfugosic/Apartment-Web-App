# Apartment-Web-App
C# Course @ FER - Final project - Apartment Web App

Tema projekta je jednostavna web aplikacija za apartman. U nastavku su opisane implementirane funkcionalnosti:

Mogućnosti gosta:
-	Pregled naslovne stranice
-	Mjenjanje jezika
-	Prijava

Mogućnosti registrirang korisnika:
-	Sve što i gost
-	Pristup posebnoj stranici za goste koja sadrži korisne informacije
-	Ostavljanje recenzije
  o	Korisnik može više puta uređivati vlastitu recenziju
  o	Korisnik može dozvoliti ili zabraniti da se njegova recenzija prikazuje na naslovnoj stranici
  
Mogućnosti administratora:
-	Sve što  i registrirani korisnik
-	Pristup posebnoj stranici za upravljanje web aplikacijom
-	Upravljanje slikama
  o	Dodavanje, brisanje i mjenjanje redoslijeda
-	Upravljanje korisničkim računima
  o	Stvaranje novog korisničkog računa za gosta
  o	Dodjeljivanje administratorskih ovlasti
  o	Brisanje računa
-	Upravljanje recenzijama
  o Stavljanje ili micanje recenzija sa naslovne stranice
  o	Brisanje recenzija

Napomene:
-	Pri pokretanju aplikacije stvara se administratorksa “uloga” (role), te glavni administrator prema postavkama u appsettings.json. Ukoliko ništa ne mjenjate, email ce biti admin@mail.com, a lozinka Admin123;
-	U appsettingsu se također može postaviti i defaultni jezik
-	Od jezika su korisniku dostupni engleski i hrvatski, njemački je također predviđen ali sakriven sa naslovne strane jer prijevod nije bio dovršen
-	Nažalost sav tekst za goste nije spreman te će se naknadno dodati

