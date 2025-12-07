# 📸 ImageGallery - Statikus Képgaléria Generátor

Ez a konzolalkalmazás egy megadott helyi könyvtárban generál, illetve abból töröl statikus HTML fájlokat. A program célja egy teljesen navigálható, **szerveroldali technológia nélküli** képgaléria létrehozása.

## ✨ Főbb Jellemzők

* **Helyi Működés (In-Place):** A generálás és a takarítás is a megadott forráskönyvtárban történik.
* **Generálás:** Rekurzívan bejárja az almappákat, létrehozva a navigációs fájlokat (`index.html` és képoldalak).
* **Takarítás (`--clear`):** Lehetővé teszi a korábban generált HTML fájlok egyszerű és gyors eltávolítását.
* **Választható Nézet (`--clear`):** Bélyegképes (thumbnail) vagy listanézet.
* **Hibakeresés (`--debug`):** Részletes logolás a konzolra a hibák nyomon követéséhez.

---

## 🚀 Használat

A programot parancssorból lehet futtatni. Mindig a **mappa elérési útja** az első kötelező argumentum.

### Szintaxis

```bash
dotnet run -- [mappa_elérési_út] [kapcsolók]
```
### 🔧 Elérhető Kapcsolók

A program működésének szabályozására a következő kapcsolókat használhatod:

| Kapcsoló | Funkció | Magyarázat |
| :--- | :--- | :--- |
| **`--clear`** | Takarítás | A megadott elérési úton és almappáiban **törli** az összes korábban generált HTML fájlt (`*.html`). |
| **`--debug`** | Naplózás | Bekapcsolja a részletes **Debug üzeneteket** a konzolon a futás során. |
| **`--showThumbs`** | Nézet | A generálás során bekapcsolja az **előnézeti képeket** (bélyegképeket) az `index.html` oldalakon. Kihagyása esetén egyszerű szöveges lista készül. |

# Bélyegképes galéria generálása
```bash
dotnet run -- /útvonal/a/képekhez --showThumbs
```
# Csak a korábban generált fájlok törlése (takarítási művelet)
```bash
dotnet run -- /útvonal/a/képekhez --clear
```
# Galéria generálása listanézettel, debug üzenetekkel
```bash
dotnet run -- /útvonal/a/képekhez --debug
```
