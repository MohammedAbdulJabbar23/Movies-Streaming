"use client";
import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import Cookies from "js-cookie";
import Loader from "./Loader";

const ProtectedRoute = ({ children, allowGuestsOnly = false }) => {
  const router = useRouter();
  const [loading, setLoading] = useState(true);
  const [mounted, setMounted] = useState(false);

  useEffect(() => {
    setMounted(true);
  }, []);

  useEffect(() => {
    if (mounted) {
      const token = Cookies.get("token"); // Check for authentication token
      if (allowGuestsOnly && token) {
        // Redirect logged-in users if allowGuestsOnly is true
        router.push("/"); // Redirect to dashboard or homepage
      } else if (!allowGuestsOnly && !token) {
        // Redirect unauthenticated users if allowGuestsOnly is false
        router.push("/login");
      } else {
        setLoading(false); // Stop loading if no redirection is required
      }
    }
  }, [mounted, router, allowGuestsOnly]);

  if (loading) {
    return (
      <div className="fixed inset-0 flex justify-center items-center bg-black/50 z-50">
        <Loader />
      </div>
    );
  }

  return <>{children}</>;
};

export default ProtectedRoute;
