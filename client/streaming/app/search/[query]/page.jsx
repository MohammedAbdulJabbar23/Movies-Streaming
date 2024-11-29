"use client";

import React, { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import axios from "axios";
import ProtectedRoute from "@/app/components/ProtectedRoute";

const SearchResults = ({ params }) => {
  const { query } = React.use(params) || {}; // Extract the dynamic query parameter
  const [results, setResults] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const apiUrl = process.env.NEXT_PUBLIC_NEW_API_URL;
  const router = useRouter();

  useEffect(() => {
    const fetchResults = async () => {
      if (!query) return;
      setLoading(true);
      setError(null);

      try {
        const response = await axios.get(
          `${apiUrl}/Movies/search?keyword=${encodeURIComponent(query)}`
        );
        setResults(response?.data);
        console.log(response.data);
      } catch (err) {
        if (err.status === 404) {
          setError("No result found !");
        } else {
          setError("Something went wrong");
        }
      } finally {
        setLoading(false);
      }
    };

    fetchResults();
  }, [query]);

  const handleOpenMovieDetails = (movieId) => {
    router.push(`/movie/${movieId}`);
  };

  return (
    <ProtectedRoute>
      <div className="p-6">
        {loading && <p>Loading...</p>}
        {error && (
          <div>
            <h1 className="w-full text-center mt-24 text-3xl font-semibold">
              Search Result
            </h1>
            <p className="text-red-500 text-lg font-medium w-full text-center mt-10">
              {error}
            </p>
          </div>
        )}
        {!error && (
          <h1 className="w-full text-center mt-24 text-3xl font-semibold">
            Search Result
          </h1>
        )}
        {!loading && !error && results.length > 0 && (
          <div className="mt-6">
            <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
              {results.map((movie) => (
                <div
                  className="relative rounded-lg overflow-hidden shadow-lg cursor-pointer"
                  key={movie.id}
                  onClick={() => handleOpenMovieDetails(movie.id)}
                >
                  <img
                    alt={movie.name}
                    className="object-cover w-full h-[200px] opacity-85 h-64"
                    src={movie.picture}
                  />
                  <div className="absolute bottom-1 left-1 right-1 bg-white/10 border border-white/20 rounded-lg p-2 flex justify-center items-center shadow-sm backdrop-blur-sm">
                    <p className="text-xs text-white">{movie.name}</p>
                  </div>
                </div>
              ))}
            </div>
          </div>
        )}
      </div>
    </ProtectedRoute>
  );
};

export default SearchResults;
