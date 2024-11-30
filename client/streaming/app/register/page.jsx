"use client";
import React, { useEffect, useState } from "react";
import axios from "axios";
import { useRouter } from "next/navigation";
import Link from "next/link";
import ProtectedRoute from "../components/ProtectedRoute";

const page = () => {
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [rePassword, setRePassword] = useState("");
  const [email, setEmail] = useState("");
  const [error, setError] = useState(null);

  const apiUrl = process.env.NEXT_PUBLIC_NEW_API_URL;
  const router = useRouter();

  useEffect(() => {
    console.log(apiUrl);
  }, []);

  const handleFormSubmit = async (e) => {
    e.preventDefault();

    if (password === rePassword) {
      try {
        const response = await axios.post(`${apiUrl}/Users/register`, {
          firstName: firstName,
          lastName: lastName,
          username: username,
          email: email,
          password: password,
        });

        if (response.status === 200 || response.status === 201) {
          setError(null);

          router.push("/login");
        } else {
          console.log("Something went wrong while registering the user");
          setError("Something went wrong");
          setRePassword("");
          setPassword("");
        }
      } catch (error) {
        console.log(error);
      }
    } else {
      setError("Make sure the passwords match");
      setPassword("");
      setRePassword("");
    }
  };

  return (
    <ProtectedRoute allowGuestsOnly>
      <div className="flex items-center justify-center min-h-screen text-gray-100">
        <div className="w-80 bg-gray-800 p-8 rounded-lg">
          <p className="text-center text-xl font-bold">Register</p>
          <form className="mt-6" onSubmit={handleFormSubmit}>
            <div className="flex space-x-4">
              <div className="mt-1 w-full">
                <label
                  htmlFor="firstName"
                  className="block text-gray-400 mb-1 text-sm"
                >
                  First Name
                </label>
                <input
                  required
                  type="text"
                  name="firstName"
                  value={firstName}
                  onChange={(e) => setFirstName(e.target.value)}
                  id="firstName"
                  className="w-full rounded-md border border-gray-700 bg-gray-800 p-2.5 text-gray-100 focus:border-purple-400 outline-none"
                />
              </div>
              <div className="mt-1 w-full">
                <label
                  htmlFor="lastName"
                  className="block text-gray-400 mb-1 text-sm"
                >
                  Last Name
                </label>
                <input
                  required
                  type="text"
                  name="lastName"
                  value={lastName}
                  onChange={(e) => setLastName(e.target.value)}
                  id="lastName"
                  className="w-full rounded-md border border-gray-700 bg-gray-800 p-2.5 text-gray-100 focus:border-purple-400 outline-none"
                />
              </div>
            </div>
            <div className="mt-4">
              <label
                htmlFor="username"
                className="block text-gray-400 mb-1 text-sm"
              >
                Username
              </label>
              <input
                required
                type="text"
                name="username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                id="username"
                className="w-full rounded-md border border-gray-700 bg-gray-800 p-2.5 text-gray-100 focus:border-purple-400 outline-none"
              />
            </div>
            <div className="mt-4">
              <label
                htmlFor="email"
                className="block text-gray-400 mb-1 text-sm"
              >
                Email
              </label>
              <input
                required
                type="email"
                name="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                id="Email"
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
                required
                type="password"
                name="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                id="password"
                className="w-full rounded-md border border-gray-700 bg-gray-800 p-2.5 text-gray-100 focus:border-purple-400 outline-none"
              />
            </div>
            <div className="mt-4">
              <label
                htmlFor="re-password"
                className="block text-gray-400 mb-1 text-sm"
              >
                Re-password
              </label>
              <input
                required
                type="password"
                name="re-password"
                value={rePassword}
                id="re-password"
                className="w-full rounded-md border border-gray-700 bg-gray-800 p-2.5 text-gray-100 focus:border-purple-400 outline-none"
                onChange={(e) => setRePassword(e.target.value)}
              />
              <div className="flex justify-end text-xs mt-2 text-gray-400">
                <a href="#" className="hover:underline">
                  Forgot Password?
                </a>
              </div>
            </div>
            <button className="w-full bg-purple-500 text-gray-900 mt-6 p-2.5 rounded-md font-semibold">
              Sign up
            </button>
          </form>

          <p className="text-center text-xs text-gray-400 mt-4">
            Already have an account?
            <Link href="/login" className="text-gray-100 ml-1 hover:underline">
              Login
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
