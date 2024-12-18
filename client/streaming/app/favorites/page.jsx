"use client";

import React, { useState, useEffect } from "react";
import axios from "axios";
import Cookies from "js-cookie";
import { useRouter } from "next/navigation";
import ProtectedRoute from "../components/ProtectedRoute";

const page = () => {

  const baseUrl = process.env.NEXT_PUBLIC_NEW_API_URL;
  const apiUrl = `${baseUrl}/Favorites`;
  const [userFavorites, setUserFavorites] = useState([]);
  const [token, setToken] = useState("");
  const router = useRouter();

  useEffect(() => {
    const userToken = Cookies.get("token");

    if (userToken) {
      setToken(userToken);
    } else {
      setToken(null);
    }
    console.log(token);
    fetchFavorites();
  }, [token]);

  const fetchFavorites = async () => {
    if (token) {
      try {
        const response = await axios.get(`${apiUrl}`, {
          params: {
            "api-version": 1,
          },
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });

        if (response.status === 200) {
          console.log("fetched favorites successfully");
          setUserFavorites(response.data);
        }
      } catch (error) {
        console.log(error);
      }
    }
  };

  const handleOpenMovieDetails = (movieId) => {
    router.push(`/movie/${movieId}`);
  };

  const onRemoveFavorite = async (movieId) => {
    if (movieId) {
      try {
        const response = await axios.delete(`${apiUrl}/${movieId}`, {
          params: {
            "api-version": 1,
          },
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });

        if (response.status === 200) {
          // Update the state to remove the deleted movie
          setUserFavorites((prevFavorites) =>
            prevFavorites.filter((movie) => movie.id !== movieId)
          );
          console.log("Movie removed successfully");
        }
      } catch (error) {}
    }
  };

  return (
    <ProtectedRoute>
      <div>
        {userFavorites.length === 0 && (
          <div className="text-2xl font-semibold uppercase h-[100vh] w-full flex justify-center items-center">
            no favorite movies added
          </div>
        )}

        {userFavorites.length !== 0 && (
          <h1 className="text-center text-2xl font-semibold mt-28">
            Favorite Movies
          </h1>
        )}
        <div className="mt-5">
          <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 p-6">
            {userFavorites &&
              userFavorites.map((movie) => {
                return (
                  <div
                    className="relative overflow-hidden shadow-lg cursor-pointer"
                    key={movie.id}
                  >
                    {/* X Icon for Removal */}
                    <button
                      onClick={(e) => {
                        e.stopPropagation(); // Prevent triggering `handleOpenMovieDetails`
                        onRemoveFavorite(movie.id); // Replace with your remove logic
                      }}
                      className="absolute top-2 right-2  text-white p-1 rounded-lg text-sm bg-black/35 w-7 hover:bg-red-700 transition duration-200"
                      title="Remove from favorites"
                    >
                      ✕
                    </button>

                    {/* Movie Poster */}
                    <img
                      alt={movie.name}
                      className="object-cover w-full h-[200px] opacity-100"
                      src={movie.picture}
                      onClick={() => handleOpenMovieDetails(movie.id)}
                    />

                    {/* Movie Name */}
                    <div className="absolute bottom-1 left-1 right-1 bg-black/50 text-white text-center p-2">
                      <p className="text-sm">{movie.name}</p>
                    </div>
                  </div>
                );
              })}
          </div>
        </div>
      </div>
    </ProtectedRoute>
  );
};

export default page;
