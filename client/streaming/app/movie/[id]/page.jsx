"use client";
import React, { useState, useEffect } from "react";
import axios from "axios";
import useFormattedDate from "@/app/hooks/useFormattedDate";

const MoviePage = ({ params }) => {
  const apiUrl = process.env.NEXT_PUBLIC_NEW_API_URL;
  const { id } = params; // Extract the dynamic route parameter

  const [movieDetails, setMovieDetails] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isVideoOpen, setIsVideoOpen] = useState(false); // Track if the video modal is open
  const [videoUrl, setVideoUrl] = useState(""); // Store the video URL for streaming

  useEffect(() => {
    const fetchMovieDetails = async () => {
      try {
        const response = await axios.get(`${apiUrl}/Movies/${id}`);
        setMovieDetails(response.data);
        setLoading(false);
      } catch (error) {
        setError("Failed to fetch movie details");
        setLoading(false);
      }
    };

    fetchMovieDetails();
  }, [id, apiUrl]);

  const handlePlayNow = async () => {
    try {
      // Fetch the streaming URL from the backend
      const response = await axios.get(`${apiUrl}/Movies/stream/${id}`, {
        responseType: "blob", // Ensure the response is a blob
      });
      
      // Log the response to check the returned data
      console.log("Received video stream:", response);
      
      const videoBlob = new Blob([response.data], { type: "video/mp4" });
      const videoUrl = URL.createObjectURL(videoBlob);

      setVideoUrl(videoUrl);
      setIsVideoOpen(true);
    } catch (error) {
      console.error("Error fetching video stream:", error);
      setError("Failed to stream the movie");
    }
  };

  const formattedReleaseDate = useFormattedDate(
    movieDetails ? movieDetails.dateCreated : ""
  );

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  if (!movieDetails) {
    return <div>No movie details available</div>;
  }

  return (
    <div className="relative w-full h-[90vh] bg-gradient-to-b from-gray-900 to-black text-white overflow-hidden">
      <div
        className="absolute inset-0"
        style={{
          backgroundImage: `url(${movieDetails.picture})`,
          backgroundSize: "cover",
          backgroundPosition: "center",
          filter: "brightness(30%)",
        }}
      />
      <div className="relative max-w-screen-xl mx-auto px-4 mt-20 sm:px-6 lg:px-8 py-12">
        <div className="flex flex-col md:flex-row items-center md:items-start">
          <img
            src={movieDetails.picture}
            alt={movieDetails.name}
            className="w-46 sm:w-64 md:w-80 h-auto md:h-96 object-cover rounded-lg shadow-2xl border border-gray-800"
          />
          <div className="md:ml-8 mt-6 md:mt-0 flex-1 text-center md:text-left">
            <h1 className="text-2xl sm:text-3xl md:text-4xl lg:text-6xl font-extrabold mb-4 leading-tight">
              {movieDetails.name}
            </h1>
            <div className="flex flex-wrap items-center justify-center md:justify-start gap-3 mb-6">
              <div className="flex items-center space-x-1">
                <span>⭐</span>
                <p className="text-lg">{movieDetails.rating}</p>
              </div>
              <div className="flex items-center space-x-1">
                <span>📅</span>
                <p className="text-lg">{formattedReleaseDate}</p>
              </div>
            </div>
            <div className="flex flex-wrap gap-2 sm:gap-3 mb-2 justify-center md:justify-start">
              <span className="bg-gray-800 text-xs sm:text-sm px-3 sm:px-4 py-1 sm:py-2 rounded-lg shadow-md">
                {movieDetails.genres.name}
              </span>
            </div>
            <div
              className="overview-container text-gray-300 text-sm sm:text-base mb-4"
              style={{ maxWidth: "700px" }}
            >
              <p className="line-clamp-3">
                Lorem ipsum dolor sit amet consectetur adipisicing elit.
                Nihil itaque nemo dolorem. Quo, esse voluptates soluta
                porro et velit enim repellat, totam temporibus amet
                tenetur incidunt at in iste id!
              </p>
            </div>
            <div className="flex flex-col sm:flex-row sm:space-x-4 space-y-4 sm:space-y-0">
              <button
                onClick={handlePlayNow}
                className="bg-white text-black px-3 sm:px-4 py-2 sm:py-3 rounded-lg shadow-lg hover:bg-gray-300 transition-colors duration-300 flex items-center justify-center w-full sm:w-auto"
              >
                <i className="fas fa-play mr-2"></i>
                Play Now
              </button>
            </div>
          </div>
        </div>
      </div>
      {/* Video Modal */}
      {isVideoOpen && (
        <div className="fixed inset-0 bg-black bg-opacity-75 flex justify-center items-center z-50">
          <div className="relative w-full max-w-4xl">
            <video
              src={videoUrl}
              controls
              autoPlay
              className="w-full rounded-lg"
            />
            <button
              onClick={() => setIsVideoOpen(false)}
              className="absolute top-2 right-2 bg-red-500 text-white px-3 py-2 rounded-full shadow-lg"
            >
              Close
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

export default MoviePage;
