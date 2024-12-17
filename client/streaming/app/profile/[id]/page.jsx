"use client";
import React, { useState, useEffect } from "react";
import axios from "axios";
import { useRouter } from "next/navigation";

const Page = ({ params }) => {
  const { id } = React.use(params);
  const [userDetails, setUserDetails] = useState({});
  const [userFavorites, setUserFavorites] = useState([]);
  const apiUrl = process.env.NEXT_PUBLIC_NEW_API_URL;
  const router = useRouter();

  useEffect(() => {
    const fetchUserDetails = async () => {
      try {
        const response = await axios.get(`${apiUrl}/Users/${id}`);
        console.log("User details:", response.data);
        setUserDetails(response.data);
      } catch (error) {
        console.log("API Error:", error.response || error.message);
      }
    };

    const fetchUserFavorite = async () => {
      try {
        const response = await axios.get(
          `http://localhost:5020/api/Favorites/user/${id}`
        );

        if (response.status === 200) {
          console.log("user favorites fetch ok ");
          setUserFavorites(response.data);
        }
      } catch (error) {
        console.log(error);
      }
    };

    if (id) {
      fetchUserDetails();
      fetchUserFavorite();
    }
  }, [id]);

  const handleOpenMovieDetails = (movieId) => {
    router.push(`/movie/${movieId}`);
  };

  return (
    <div className="flex flex-col items-center p-6 min-h-screen mt-24">
      <div className="bg-gray-800 shadow-md rounded-lg p-6 w-full max-w-md">
        <div className="flex flex-col items-center">
          <img
            src="https://st3.depositphotos.com/9998432/13335/v/450/depositphotos_133352010-stock-illustration-default-placeholder-man-and-woman.jpg"
            alt="User Profile"
            className="w-24 h-24 rounded-full mb-4"
          />
          <h1 className="text-xl font-bold text-gray-100">
            {userDetails.firstName || "First Name"}{" "}
            {userDetails.lastName || "Last Name"}
          </h1>
          <p className="text-gray-400">@{userDetails.userName || "username"}</p>
        </div>

        <div className="mt-6">
          <div className="text-gray-200">
            <p className="mb-2">
              <span className="font-medium">Email: </span>
              {userDetails.email || "user@example.com"}
            </p>
          </div>
        </div>
      </div>
      {/* favorites  */}
      <div>
        {userFavorites.length === 0 && (
          <div className="text-2xl font-semibold uppercase w-full flex justify-center items-center mt-12">
            no favorite movies
          </div>
        )}

        {userFavorites.length !== 0 && (
          <h1 className="text-center text-2xl font-semibold mt-10">
            Favorite Movies
          </h1>
        )}
        <div className="mt-5">
          <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-4 p-6">
            {userFavorites &&
              userFavorites.map((movie) => {
                return (
                  <div
                    className="relative overflow-hidden shadow-lg cursor-pointer"
                    key={movie.id}
                  >
                    {/* Movie Poster */}
                    <img
                      alt={movie.name}
                      className="object-cover w-full h-[300px] opacity-100"
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
    </div>
  );
};

export default Page;
