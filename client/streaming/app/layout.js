"use client";
import "./globals.css";
import Header from "./components/Header";
import AiChatButton from "./components/AiChatButton";
import { usePathname } from "next/navigation";
import  UploadMovieButton  from "./components/UploadMovieButton";

export default function RootLayout({ children }) {
  const pathname = usePathname();

  // Paths where the header should show
  const showHeaderPaths = ["/", "/profile"];
  // Paths where the header should hide
  const hideHeaderPaths = ["/login", "/register"];
  const chatPagePath = "/ai-chat";
  const uploadMoviePath = '/upload-movie';

  // add the dynamic paths that should show the header
  const shouldShowHeader =
    showHeaderPaths.some((path) => pathname.startsWith(path)) ||
    pathname.startsWith("/movie") ||
    pathname.startsWith("/search");

  const shouldHideHeader = hideHeaderPaths.includes(pathname);

  // Default to hiding the header if the path isn’t in either list
  const showHeader = shouldShowHeader && !shouldHideHeader;

  const shouldShowAiChatButton = showHeader && pathname !== chatPagePath;
  const shouldShowUploadButton = showHeader && pathname !== uploadMoviePath;

  return (
    <html lang="en">
      <head>
        <link
          href="https://fonts.googleapis.com/css2?family=Montserrat:wght@400;700&display=swap"
          rel="stylesheet"
        />
        <link
          rel="stylesheet"
          href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css"
        />
      
      </head>
      <body className="bg-[#02111B] font-sans">
        <div className="flex justify-center">
          {showHeader && <Header />} {/* Conditionally render the Header */}
          {shouldShowAiChatButton && <AiChatButton />}
        </div>
        {children}
      </body>
    </html>
  );
}
