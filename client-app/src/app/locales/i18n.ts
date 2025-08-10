import i18n from "i18next";
import { initReactI18next } from "react-i18next";
import LanguageDetector from "i18next-browser-languagedetector";

i18n
  .use(LanguageDetector)
  .use(initReactI18next)
  .init({
    resources: {
      en: {
        translation: {
          // Basic app translations
          addPost: "Add Post",
          category: "Category",
          categories: "Categories",
          profile: "Profile",
          changeLanguage: "Zmień na PL",
          notLogin: "Login",
          login: "Login",
          loginSuccess: "You have logged in successfully",
          loginFailed: "Login failed",
          logout: "Logout",
          logoutSuccess: "You have logged out successfully",
          logoutFailed: "Logout failed",
          orLogin: "Or login with",
          orRegister: "Or sign in",
          signIn: "Sign in",
          password: "Password",
          passwordConfirm: "Confirm Password",
          firstName: "First Name",
          lastName: "Last Name",
          country: "Country",
          city: "City",
          street: "Street",
          houseNumber: "House number",
          postalCode: "Postal code",
          passwordConfirmFailed: "Password field mismatch",
          signInSuccess: "You have signed in successfully",
          signInFailed: "Sign in has failed",
          settings: "Settings",
          thankForSignIn: "Thank you for sign in!",

          // Email confirmation translations
          emailConfirmation:
            "To complete your registration, please confirm your email address by clicking the link in the email we sent you. If you didn't receive the email, please check your spam folder or click below to resend the email.",
          resendEmailConfirmation: "Resend email",
          confirmationMailSent: "New confirmation mail has been sent",
          confirmationMailSuccess: "Email has confirmed successfully",
          confirmationMailFailed: "Email confirmation failed",
          confirmationMailError: "Error while sending mail",
          sendAgain: "Send again in",

          // General UI translations
          hello: "Hello",
          feed: "Feed",
          popular: "Popular",
          loading: "Loading",
          negotiable: "Negotiable",
          Free: "Free",
          from: "from",
          upTo: "up to",
          failedToLoadPosts: "Failed to load posts",
          postNotFound: "Post not found",
          failedToLoadPost: "Failed to load post",
          propertyDetails: "Property Details",
          noPosts: "No posts available",

          // Work experience translations
          ExperienceRequired: "Experience required",
          ExperienceNotRequired: "No experience required",

          // Property translations
          rooms: "Number of rooms",
          floor: "Floor",

          // Price type translations
          "Onetime Payment": "One-time Payment",
          "Per Hour": "Per hour",
          "Per Month": "Per month",
          "Per Day": "Per day",
          "Per Item": "Per item",

          // Property type translations
          WareHouse: "Warehouse",
          Room: "Room",
          Other: "Other",
          House: "House",
          Apartment: "Apartment",
          Office: "Office",
          Studio: "Studio",

          // Work type translations
          "Full-Time": "Full-time",
          "Part-Time": "Part-time",
          Freelance: "Freelance",
          Internship: "Internship",
          Shift: "Shift",
          Seasonal: "Seasonal",

          // Contract type translations
          "Employment Contract": "Employment contract",
          "Specific Work Contract": "Specific Work Contract",
          "Task Contract": "Task contract",
          B2B: "B2B",
          "Volunteer Contract": "Volunteer contract",

          // Work location translations
          OnSite: "On-site",
          Remote: "Remote",
          Hybrid: "Hybrid",

          // Navigation translations
          Home: "Home",
          home: "Home",
          search: "Search",
          searchPlaceholder: "Search posts, categories...",
          notifications: "Notifications",
          user: "User",
          menu: "Menu",
          pleaseLoginToAddPost: "Please login to add a post",
          loginToAddPost: "Login to add",
          switchToLight: "Switch to light theme",
          switchToDark: "Switch to dark theme",
          favorites: "Favorites",
          savedPosts: "Saved Posts",
          saved: "Saved",
          history: "History",
          help: "Help",
          about: "About",
          darkMode: "Dark mode",
          language: "Language",
          currentLanguage: "EN",
          loginRequired: "Login required",
          allRightsReserved: "All rights reserved",
          invalidConfirmationLink: "Invalid confirmation link",

          // Categories translations
          failedToLoadCategories: "Failed to load categories",
          noCategoriesFound: "No categories found",
          subcategory: "subcategory",
          subcategories: "subcategories",
          error: "Error",
          browseCategoriesDescription: "Browse all available categories",
          explore: "Explore",

          // Email confirmation page specific
          emailConfirm: {
            verifying: "Verifying your email...",
            verifyingDescription:
              "Please wait while we confirm your email address.",
            success: "Email Verified!",
            successDescription: "Your email has been successfully verified.",
            accountActivated: "Your account is now active and ready to use.",
            redirecting: "Redirecting to login page in {{seconds}} seconds...",
            goToLogin: "Go to Login",
            goHome: "Home Page",
            failed: "Verification Failed",
            failedDescription: "We couldn't verify your email address.",
            possibleReasons:
              "This may be due to an expired or invalid link. Try requesting a new verification email.",
            tryLogin: "Try to Login",
            invalidLink: "Invalid Link",
            invalidLinkDescription:
              "The confirmation link appears to be invalid or corrupted.",
            invalidLinkHelp:
              "Please check the link in your email or request a new verification email from the login page.",
            thankYou: "Thank you!",
            registrationComplete: "Your registration is almost complete",
            checkInbox:
              "We've sent a verification link to your email address. Check your inbox and click the link to activate your account.",
            instructions:
              "To complete your registration, check your email and click the verification link. The link will expire in 24 hours for security reasons.",
            didntReceive: "Didn't receive the email?",
            resendEmail: "Resend Verification Email",
            resendIn: "You can resend the email in",
            resendAvailable: "You can resend the email in",
            sending: "Sending...",
            emailsSent: "Verification emails sent: {{count}}",
            backToLogin: "Back to Login",
            checkSpamTip:
              "Don't forget to check your spam/junk folder if you don't see the email in your inbox.",
            noEmailProvided: "No email address provided",
          },

          // Auth translations
          auth: {
            welcome: "Welcome",
            subtitle: "Login to your account or create a new one",
            login: "Login",
            register: "Register",
            email: "Email",
            password: "Password",
            confirmPassword: "Confirm Password",
            firstName: "First Name",
            lastName: "Last Name",
            loginSuccess: "Successfully logged in!",
            loginError: "Login failed. Please check your credentials.",
            registerSuccess:
              "Registration successful! Please check your email to verify your account.",
            registerError: "Registration failed. Please try again.",
            googleLoginSuccess: "Successfully logged in with Google!",
            googleLoginError: "Google login failed. Please try again.",
            continueWithGoogle: "Continue with Google",
            or: "OR",
          },

          postImage: "Post image",
          viewAllPhotos: "View all photos",
          imageGallery: "Image Gallery",
          of: "of",
          loadingImages: "Loading images",
          noImagesAvailable: "No images available",
          imageLoadError: "Failed to load image",
          image: "Image",
          fullscreen: "Fullscreen",

          // Post interactions
          share: "Share",
          favorite: "Add to favorites",
          views: "views",
          anonymous: "Anonymous",
          trending: "Trending",
          posts: "posts",
          posts_one: "post",
          posts_other: "posts",

          // Work and service types
          work: "Work",
          rent: "Rent",
          service: "Service",

          // Enhanced UI
          noPostsDescription: "Check back later for new posts",
          
          // Promotion types
          promotion: {
            Highlight: "Highlighted",
            Top: "Top Post",
          },

          // Validation translations
          validation: {
            required: "This field is required",
            invalidEmail: "Please enter a valid email address",
            passwordMinLength: "Password must be at least 6 characters long",
            nameMinLength: "Name must be at least 2 characters long",
            passwordsDoNotMatch: "Passwords do not match",
          },
        },
      },
      pl: {
        translation: {
          // Basic app translations
          addPost: "Dodaj Ogłoszenie",
          category: "Kategoria",
          categories: "Kategorie",
          profile: "Profil użytkownika",
          changeLanguage: "Change to EN",
          notLogin: "Logowanie",
          login: "Zaloguj się",
          loginSuccess: "Zalogowałeś się pomyślnie",
          loginFailed: "Nieudana próba logowania",
          logout: "Wyloguj się",
          logoutSuccess: "Zostałeś pomyślnie wylogowany",
          logoutFailed: "Nieudana próba wylogowania",
          orLogin: "Lub zaloguj się za pomocą",
          orRegister: "Lub zarejestruj się",
          signIn: "Zarejestruj się",
          password: "Hasło",
          passwordConfirm: "Potwierdź hasło",
          firstName: "Imię",
          lastName: "Nazwisko",
          country: "Państwo",
          city: "Miasto",
          street: "Ulica",
          houseNumber: "Numer budynku",
          postalCode: "Kod pocztowy",
          passwordConfirmFailed: "Hasło się nie zgadza",
          signInSuccess: "Rejestracja zakończona pomyślnie",
          signInFailed: "Nieudana próba rejestracji",
          settings: "Ustawienia",
          thankForSignIn: "Dziękujemy za rejestrację!",

          // Email confirmation translations
          emailConfirmation:
            "Aby zakończyć rejestrację, prosimy o potwierdzenie swojego adresu e-mail, klikając link w wiadomości, którą wysłaliśmy. Jeśli nie otrzymałeś wiadomości, sprawdź folder ze spamem lub kliknij poniżej, aby wysłać wiadomość ponownie.",
          resendEmailConfirmation: "Wyślij ponownie",
          confirmationMailSent: "Nowa wiadomość potwierdzająca została wysłana",
          confirmationMailSuccess: "Adres email został potwierdzony",
          confirmationMailFailed: "Nieudana próba potwierdzenia adresu email",
          confirmationMailError: "Nieudana próba wysłania",
          sendAgain: "Wyślij ponownie za",

          // General UI translations
          hello: "Witaj",
          feed: "Polecane",
          popular: "Popularne",
          loading: "Ładowanie",
          negotiable: "Do uzgodnienia",
          Free: "Darmowe",
          from: "od",
          upTo: "do",
          failedToLoadPosts: "Nie udało się załadować ogłoszeń",
          postNotFound: "Ogłoszenie nie znalezione",
          failedToLoadPost: "Nie udało się załadować ogłoszenia",
          propertyDetails: "Szczegóły nieruchomości",
          noPosts: "Brak dostępnych ogłoszeń",

          // Work experience translations
          ExperienceRequired: "Wymagane doświadczenie",
          ExperienceNotRequired: "Bez doświadczenia",

          // Property translations
          rooms: "Liczba pokoi",
          floor: "Piętro",

          // Price type translations
          "Onetime Payment": "Płatność jednorazowa",
          "Per Hour": "Za godzinę",
          "Per Month": "Za miesiąc",
          "Per Day": "Za dzień",
          "Per Item": "Za sztukę",

          // Property type translations
          WareHouse: "Magazyn",
          Room: "Pokój",
          Other: "Inne",
          House: "Dom",
          Apartment: "Mieszkanie",
          Office: "Biuro",
          Studio: "Kawalerka",

          // Work type translations
          "Full-Time": "Pełny etat",
          "Part-Time": "Niepełny etat",
          Freelance: "Freelance",
          Internship: "Praktyki",
          Shift: "Zmianowa",
          Seasonal: "Sezonowa",

          // Contract type translations
          "Employment Contract": "Umowa o pracę",
          "Task Contract": "Umowa zlecenie",
          "Specific Work Contract": "Umowa o dzieło",
          "Volunteer Contract": "Umowa wolontariacka",
          B2B: "B2B",

          // Work location translations
          OnSite: "W biurze",
          Remote: "Zdalnie",
          Hybrid: "Hybrydowo",

          // Navigation translations
          Home: "Strona Główna",
          home: "Strona główna",
          search: "Szukaj",
          searchPlaceholder: "Szukaj ogłoszeń, kategorii...",
          notifications: "Powiadomienia",
          user: "Użytkownik",
          menu: "Menu",
          pleaseLoginToAddPost: "Zaloguj się, aby dodać ogłoszenie",
          loginToAddPost: "Zaloguj się, aby dodać",
          switchToLight: "Przełącz na jasny motyw",
          switchToDark: "Przełącz na ciemny motyw",
          favorites: "Ulubione",
          savedPosts: "Zapisane Ogłoszenia",
          saved: "Zapisane",
          history: "Historia",
          help: "Pomoc",
          about: "O nas",
          darkMode: "Tryb ciemny",
          language: "Język",
          currentLanguage: "PL",
          loginRequired: "Wymagane logowanie",
          allRightsReserved: "Wszystkie prawa zastrzeżone",
          invalidConfirmationLink: "Nieprawidłowy link potwierdzający",

          // Categories translations
          failedToLoadCategories: "Nie udało się załadować kategorii",
          noCategoriesFound: "Nie znaleziono kategorii",
          subcategory: "podkategoria",
          subcategories: "podkategorie",
          error: "Błąd",
          browseCategoriesDescription: "Przeglądaj wszystkie dostępne kategorie",
          explore: "Eksploruj",

          // Email confirmation page specific
          emailConfirm: {
            verifying: "Weryfikowanie adresu email...",
            verifyingDescription:
              "Proszę czekać, podczas gdy potwierdzamy Twój adres email.",
            success: "Email Zweryfikowany!",
            successDescription: "Twój email został pomyślnie zweryfikowany.",
            accountActivated: "Twoje konto jest teraz aktywne i gotowe do użycia.",
            redirecting: "Przekierowanie do strony logowania za {{seconds}} sekund...",
            goToLogin: "Przejdź do Logowania",
            goHome: "Strona Główna",
            failed: "Weryfikacja Nieudana",
            failedDescription: "Nie mogliśmy zweryfikować Twojego adresu email.",
            possibleReasons:
              "Może to być spowodowane wygasłym lub nieprawidłowym linkiem. Spróbuj poprosić o nowy email weryfikacyjny.",
            tryLogin: "Spróbuj się Zalogować",
            invalidLink: "Nieprawidłowy Link",
            invalidLinkDescription:
              "Link potwierdzający wydaje się być nieprawidłowy lub uszkodzony.",
            invalidLinkHelp:
              "Sprawdź link w swoim emailu lub poproś o nowy email weryfikacyjny ze strony logowania.",
            thankYou: "Dziękujemy!",
            registrationComplete: "Twoja rejestracja jest prawie zakończona",
            checkInbox:
              "Wysłaliśmy link weryfikacyjny na Twój adres email. Sprawdź swoją skrzynkę i kliknij link, aby aktywować konto.",
            instructions:
              "Aby zakończyć rejestrację, sprawdź swój email i kliknij link weryfikacyjny. Link wygaśnie za 24 godziny ze względów bezpieczeństwa.",
            didntReceive: "Nie otrzymałeś emaila?",
            resendEmail: "Wyślij ponownie Email Weryfikacyjny",
            resendIn: "Możliwość ponownego wysłania za",
            resendAvailable: "Możesz wysłać ponownie email za",
            sending: "Wysyłanie...",
            emailsSent: "Wysłane emaile weryfikacyjne: {{count}}",
            backToLogin: "Powrót do Logowania",
            checkSpamTip:
              "Nie zapomnij sprawdzić folderu spam/niechciane, jeśli nie widzisz emaila w skrzynce odbiorczej.",
            noEmailProvided: "Nie podano adresu email",
          },

          // Auth translations
          auth: {
            welcome: "Witamy",
            subtitle: "Zaloguj się do swojego konta lub utwórz nowe",
            login: "Logowanie",
            register: "Rejestracja",
            email: "Email",
            password: "Hasło",
            confirmPassword: "Potwierdź hasło",
            firstName: "Imię",
            lastName: "Nazwisko",
            loginSuccess: "Pomyślnie zalogowano!",
            loginError: "Logowanie nie powiodło się. Sprawdź swoje dane.",
            registerSuccess:
              "Rejestracja zakończona pomyślnie! Sprawdź swoją skrzynkę email, aby zweryfikować konto.",
            registerError: "Rejestracja nie powiodła się. Spróbuj ponownie.",
            googleLoginSuccess: "Pomyślnie zalogowano przez Google!",
            googleLoginError:
              "Logowanie przez Google nie powiodło się. Spróbuj ponownie.",
            continueWithGoogle: "Kontynuuj z Google",
            or: "LUB",
          },

          // Promotion types
          promotion: {
            Highlight: "Wyróżnione",
            Top: "Top Ogłoszenie",
          },

          postImage: "Zdjęcie ogłoszenia",
          viewAllPhotos: "Zobacz wszystkie zdjęcia",
          imageGallery: "Galeria Zdjęć",
          of: "z",
          loadingImages: "Ładowanie zdjęć",
          noImagesAvailable: "Brak dostępnych zdjęć",
          imageLoadError: "Nie udało się załadować zdjęcia",
          image: "Zdjęcie",
          fullscreen: "Pełny ekran",

          // Post interactions
          share: "Udostępnij",
          favorite: "Dodaj do ulubionych",
          views: "wyświetleń",
          anonymous: "Anonim",
          trending: "Na czasie",
          posts: "ogłoszeń",
          posts_one: "ogłoszenie",
          posts_other: "ogłoszeń",

          // Work and service types
          work: "Praca",
          rent: "Wynajem",
          service: "Usługa",

          // Enhanced UI
          noPostsDescription:
            "Sprawdź ponownie później, aby zobaczyć nowe ogłoszenia",

          // Validation translations
          validation: {
            required: "To pole jest wymagane",
            invalidEmail: "Wprowadź prawidłowy adres email",
            passwordMinLength: "Hasło musi mieć co najmniej 6 znaków",
            nameMinLength: "Imię musi mieć co najmniej 2 znaki",
            passwordsDoNotMatch: "Hasła się nie zgadzają",
          },
        },
      },
    },
    lng: localStorage.getItem("brt_lng") || "en",
    fallbackLng: "en",
    interpolation: { escapeValue: false },
  });

export default i18n;