"use client";
import React from "react";
import axios from "axios";
import { useState } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import Cookies from "js-cookie";
import ProtectedRoute from "../components/ProtectedRoute";

const page = () => {
  const [toket, setToken] = useState("");
  const [username, setUserName] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState(null);

  const router = useRouter();

  const apiUrl = process.env.NEXT_PUBLIC_NEW_API_URL;

  const handleFormSubmit = async (e) => {
    e.preventDefault();

    try {
      const response = await axios.post(
        `${apiUrl}/Users/authenticate`,
        {
          username,
          password,
        },
        { cache: "no-cache" }
      );

      if (response.status === 200) {
        console.log("Login OK");
        setError(null);
        console.log(response);
        Cookies.set("token", response.data.token, { path: "/" });
        router.push("/");
      } else {
        console.log("RESPONSE IS NOT OK ");
        setError("please double-check your credentials");
        setPassword("");
      }
    } catch (error) {
      setError("Error: Invalid credentials");
      console.log(error);
      setPassword("");
    }
  };

  return (
    <ProtectedRoute allowGuestsOnly>
      <div className="flex items-center justify-center min-h-screen text-gray-100">
        <div className="w-80 bg-gray-800 p-8 rounded-lg">
          <p className="text-center text-xl font-bold">Login</p>
          <form className="mt-6" onSubmit={handleFormSubmit}>
            <div className="mt-1">
              <label
                htmlFor="username"
                className="block text-gray-400 mb-1 text-sm"
              >
                Username
              </label>
              <input
                type="text"
                name="username"
                value={username}
                onChange={(e) => setUserName(e.target.value)}
                id="username"
                className="w-full rounded-md border border-gray-700 bg-gray-800 p-2.5 text-gray-100 focus:border-purple-400 outline-none"
              />
            </div>
            <div className="mt-4">
              <label
                htmlFor="password"
                className="block text-gray-400 mb-1 text-sm"
              >
                Password
              </label>
              <input
                type="password"
                name="password"
                value={password}
                id="password"
                className="w-full rounded-md border border-gray-700 bg-gray-800 p-2.5 text-gray-100 focus:border-purple-400 outline-none"
                onChange={(e) => setPassword(e.target.value)}
              />
              <div className="flex justify-end text-xs mt-2 text-gray-400">
                <a href="#" className="hover:underline">
                  Forgot Password?
                </a>
              </div>
            </div>
            <button className="w-full bg-purple-500 text-gray-900 mt-6 p-2.5 rounded-md font-semibold">
              Sign in
            </button>
          </form>
          {/* <div className="flex items-center my-4">
          <div className="flex-grow h-px bg-gray-700" />
          <p className="px-3 text-sm text-gray-400">
            Login with social accounts
          </p>
          <div className="flex-grow h-px bg-gray-700" />
        </div> */}

          <p className="text-center text-xs text-gray-400 mt-4">
            Don't have an account?
            <Link
              href="/register"
              className="text-gray-100 ml-1 hover:underline"
            >
              Sign up
            </Link>
          </p>
          {error && (
            <p className="text-center text-sm text-red-500 mt-4">{error}</p>
          )}
        </div>
      </div>
    </ProtectedRoute>
  );
};

export default page;
