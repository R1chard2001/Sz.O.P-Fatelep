# Streamalapú kommunikáció: Fatelep

Készíts egy kliens-szerver alkalmazást egy fatelep rendszerének megvalósítására!
A szerver egyszerre több klienst is tudjon kiszolgálni!

A kliens képes legyen konzolról küldeni a szervernek bármilyen utasítást! Legyen felkészülve arra, hogy a szerver nem csak egysoros, de többsoros választ is küldhet!

A szervernek kezelnie kell felhasználókat. A felhasználók adatait egy XML fájl tartalmazza, melyet Neked kell összeállítanod pár user adatával. A fájlban egy felhasználó adatai egy sorban legyenek leírva, <user name="..." password="..." isAdmin="..."/> alakban. A szerver induláskor olvassa be az adatokat!

A fatelep készletén levő deszkák adatait kell beolvasni a timber.xml fájlból. A szerver indulásakor olvassa be ezeket is!

Megjegyzés: A price-ban a deszkák folyóméterenkénti (fm) ára van megadva, azaz hogy 1 m hosszú deszka mennyibe kerül.

A szervernek támogatni kell az alábbi funkciókat:

 - LOGIN|\<usernév>|\<jelszó>: Bejelentkezteti az adott felhasználót.
 - LOGOUT: Kijelentkezteti az adott klienst a szerverről.
 - LIST|\<szélesség>: Kilistázza a fatelepen található összes, adott szélességű léc adatait.
 - PRICE|\<fafaj>|\<szélesség>|\<fm>: Kiírja, hogy az adott fajtájú lécből mennyibe kerül ennyi fm. Ha nincs ilyen fajta deszka készleten, arról értesít.
 - EXIT: A kliens kilép.

A következő funkció csak bejelentkezés után érhető el:

 - ADD|\<fafaj>|\<szélesség>|\<fm_ár>: Ha a készleten van már ilyen fából készült ilyen szélességű deszka, akkor frissíti az árát a megadottra. Ha még nem létezik, akkor új deszkaként hozzáadja a készlethez.

A következő funciók csak adminoknak elérhetőek:

 - USERS: Usernevek listázása.
 - ADDUSER|\<usernév>|\<jelszó>: Új felhasználó felvétele.
 - DELUSER|\<usernév>: Felhasználó törlése.
 - ONLINEUSERS: Online userek listázása. Kiírja, kik vannak aktuálisan bejelentkezve a szerverre.
