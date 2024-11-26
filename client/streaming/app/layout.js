"use client";
import "./globals.css";
import Header from "./components/Header";
import { usePathname } from "next/navigation";

export default function RootLayout({ children }) {
  const pathname = usePathname();

  // Paths where the header should show
  const showHeaderPaths = ["/", "/profile"];
  // Paths where the header should hide
  const hideHeaderPaths = ["/login", "/register"];

  // add the dynamic paths that should show the header
  const shouldShowHeader =
    showHeaderPaths.some((path) => pathname.startsWith(path)) ||
    pathname.startsWith("/movie") ||
    pathname.startsWith("/search");

  const shouldHideHeader = hideHeaderPaths.includes(pathname);

  // Default to hiding the header if the path isn’t in either list
  const showHeader = shouldShowHeader && !shouldHideHeader;

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
        </div>
        {children}
      </body>
    </html>
  );
}
