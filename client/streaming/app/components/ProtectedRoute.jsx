"use client";
import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import Cookies from "js-cookie";
import Loader from "./Loader";

const ProtectedRoute = ({ children }) => {
  const router = useRouter();
  const [loading, setLoading] = useState(true); // Start with loading set to true
  const [mounted, setMounted] = useState(false); // Track whether the component has mounted

  // Run this effect on mount
  useEffect(() => {
    setMounted(true); // Component has mounted
  }, []);

  useEffect(() => {
    // Prevents router usage on the server-side, and only runs on the client-side
    if (mounted) {
      const token = Cookies.get("token"); // Check for authentication token in cookies
      if (!token) {
        router.push("/login"); // If no token is found, redirect to the login page
      } else {
        setLoading(false); // If token exists, stop loading
      }
    }
  }, [mounted, router]);

  // Show a loading spinner or message while checking for authentication
  if (loading) {
    return (
      <div className="fixed inset-0 flex justify-center items-center bg-black/50 z-50">
        <Loader />
      </div>
    );
  }

  // Render the protected content once authentication is complete
  return <>{children}</>;
};

export default ProtectedRoute;
