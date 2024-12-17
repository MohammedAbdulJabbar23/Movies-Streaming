"use client";
import React, { useState, useEffect } from "react";
import axios from "axios";
import Loader from "@/app/components/Loader";
import useFormattedDate from "@/app/hooks/useFormattedDate";
import ProtectedRoute from "@/app/components/ProtectedRoute";
import CommentForm from "@/app/components/CommentForm";
import Link from "next/link";
import Cookies from "js-cookie";

const MoviePage = ({ params }) => {
  const apiUrl = process.env.NEXT_PUBLIC_NEW_API_URL;
  const { id } = React.use(params); // Extract the dynamic route parameter

  const [movieDetails, setMovieDetails] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isVideoOpen, setIsVideoOpen] = useState(false); // Track if the video modal is open
  const [videoUrl, setVideoUrl] = useState(""); // Store the video URL for streaming
  const [comments, setComments] = useState([]);
  const [showCommentForm, setShowCommentForm] = useState(false);
  const [token, setToken] = useState("");
  const [notification, setNotification] = useState(null);
  const [suggestedMovies, setSuggestedMovies] = useState([]);

  useEffect(() => {
    const userToken = Cookies.get("token");
    if (userToken) {
      setToken(userToken);
    }
  }, []);

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

    const fetchMovieComments = async () => {
      try {
        const response = await axios.get(
          `http://localhost:5020/api/Comment/movie/${id}`
        );

        // Format comments with formatted dates before setting state
        if (response.status === 200) {
          setComments(response.data);
        }
      } catch (error) {
        setError("Failed to fetch movie comments");
        console.log(error);
      }
    };

    fetchMovieDetails();
    fetchMovieComments();
  }, [id]);

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

  const formattedMovieReleaseDate = useFormattedDate(
    movieDetails ? movieDetails.dateCreated : ""
  );

  if (loading) {
    return (
      <div className="fixed inset-0 flex justify-center items-center bg-black/50 z-50">
        <Loader />
      </div>
    );
  }

  if (error) {
    return <div>{error}</div>;
  }

  if (!movieDetails) {
    return <div>No movie details available</div>;
  }

  const autoDismissNotification = () => {
    setTimeout(() => {
      setNotification(null);
    }, 2000);
  };

  const handleAddToFavorites = async () => {
    if (token) {
      try {
        const response = await axios.post(
          `http://localhost:5020/api/Favorites/${movieDetails.id}`,
          {},
          {
            params: {
              "api-version": 1,
            },
            headers: {
              Authorization: `Bearer ${token}`,
              "Content-Type": "application/json",
            },
          }
        );

        if (response.status === 200) {
          console.log("movie added");
          setNotification({
            message: "Movie added to favorites!",
            type: "success",
          });
          autoDismissNotification();
        }
      } catch (error) {
        if (error.response?.status === 400) {
          setNotification({
            message: "Movie is already in your favorites!",
            type: "error",
          });
        } else {
          setNotification({
            message: "An error occurred. Please try again.",
            type: "error",
          });
        }
        autoDismissNotification();
        console.log(error);
      }
    } else {
      console.log("no token found");
    }
  };

  // const handleSuggestMovies = async () => {
  //   try {
  //     const response = await axios.post(
  //       "http://localhost:5020/api/MovieSuggestions/suggest-similar-movies",
  //       {
  //         movieName: movieDetails.name,
  //       }
  //     );

  //     if (response.status === 200) {
  //       setSuggestedMovies(response.data.suggestions);
  //       console.log("response is ok");
  //       console.log(response.data.suggestions);
  //     }
  //   } catch (error) {
  //     console.log(error);
  //   }
  // };

  return (
    <ProtectedRoute>
      <div className="relative w-full bg-gradient-to-b from-gray-900 to-black text-white overflow-hidden">
        {/* Notification Popup */}
        {notification && (
          <div
            className={`fixed top-4 right-4 px-6 py-3 rounded-md text-white text-md shadow-lg z-10 ${
              notification.type === "success" ? "bg-green-500" : "bg-red-500"
            }`}
          >
            {notification.message}
          </div>
        )}
        <div
          className="absolute inset-0 h-[90vh]"
          style={{
            backgroundImage: `url(${movieDetails.picture})`,
            backgroundSize: "cover",
            backgroundPosition: "center",
            filter: "brightness(30%)",
          }}
        />
        <div className="relative max-w-screen-xl mx-auto px-4 mt-20 sm:px-6 lg:px-8 py-12 h-full">
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
                  <p className="text-lg">
                    {movieDetails.rating ? movieDetails.rating : 4.4}
                  </p>
                </div>
                <div className="flex items-center space-x-1">
                  <span>📅</span>
                  <p className="text-lg">
                    {formattedMovieReleaseDate
                      ? formattedMovieReleaseDate
                      : "1970/1/1"}
                  </p>
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
                  {movieDetails.description
                    ? movieDetails.description
                    : "Lorem ipsum dolor sit amet consectetur adipisicing elit. Nihil"}
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
                <button
                  onClick={handleAddToFavorites}
                  className="bg-white text-black px-3 sm:px-4 py-2 sm:py-3 rounded-lg shadow-lg hover:bg-gray-300 transition-colors duration-300 flex items-center justify-center w-full sm:w-auto"
                >
                  <i className="fas fa-heart mr-2"></i>
                  Add to Favorites
                </button>
              </div>
            </div>
          </div>
        </div>

        {/* comment section */}

        <section className="pt-5 px-10 pb-6 w-[100vw] mt-[40vh] mb-[10vh]">
          <span className="flex justify-between px-10">
            <h1 className="text-2xl font-semibold">Comments</h1>
            {showCommentForm ? (
              <CommentForm
                setShowCommentForm={setShowCommentForm}
                showCommentForm={showCommentForm}
                id={id}
                comments={comments}
                setComments={setComments}
              />
            ) : (
              <button
                className="border text-sm p-2 hover:text-black hover:bg-white ease-in-out duration-300"
                onClick={() => setShowCommentForm(!showCommentForm)}
              >
                + Add Comment
              </button>
            )}
          </span>
          <hr className="text-slate-500 opacity-50 mt-3 w-inherit" />

          {comments?.map((comment) => {
            return (
              <div
                className="mt-3 flex justify-between items-center px-10 mt-5"
                key={comment.id}
              >
                <span className="flex items-center gap-4">
                  <Link href={`/profile/${comment.id}`}>
                    <img
                      src="https://th.bing.com/th/id/OIP.SAcV4rjQCseubnk32USHigHaHx?rs=1&pid=ImgDetMain"
                      alt="user's pic"
                      className="w-10 h-10 rounded-full object-cover"
                    />
                  </Link>

                  <span>
                    <p className="text-xs font-semibold">{comment.userName}</p>
                    <p className="text-sm">{comment.text}</p>
                  </span>
                </span>
                <span className="text-xs opacity-50">{comment.createdAt}</span>
              </div>
            );
          })}
        </section>

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

        {/* suggest movies section  */}
        {/* <div className="w-full m-4">
          <div className="flex justify-center items-center">
            <button
              className="group flex items-center justify-center gap-3 p-4 rounded-full bg-[#1C1A1C] cursor-pointer transition-all  duration-300 ease-in-out hover:bg-gradient-to-t hover:from-[#A47CF3] hover:to-[#683FEA] hover:shadow-[inset_0px_1px_0px_rgba(255,255,255,0.4),inset_0px_-4px_0px_rgba(0,0,0,0.2),0px_0px_0px_4px_rgba(255,255,255,0.2),0px_0px_180px_0px_#9917FF] hover:translate-y-[-2px]"
              onClick={handleSuggestMovies}
            >
              <svg
                height={24}
                width={24}
                fill="#AAAAAA"
                viewBox="0 0 24 24"
                data-name="Layer 1"
                id="Layer_1"
                className="sparkle transition-all duration-700 ease group-hover:fill-white group-hover:scale-125"
              >
                <path d="M10,21.236,6.755,14.745.264,11.5,6.755,8.255,10,1.764l3.245,6.491L19.736,11.5l-6.491,3.245ZM18,21l1.5,3L21,21l3-1.5L21,18l-1.5-3L18,18l-3,1.5ZM19.333,4.667,20.5,7l1.167-2.333L24,3.5,21.667,2.333,20.5,0,19.333,2.333,17,3.5Z" />
              </svg>
              <span className="text font-semibold text-[#AAAAAA] text-base duration-300 transition-colors group-hover:text-white">
                Generate Similar Movies
              </span>
            </button>
          </div>

          <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-4 p-4 mr-8 mt-4">
            {suggestedMovies &&
              suggestedMovies.map((movie) => (
                <div
                  key={movie.movieName}
                  className="p-4 border border-gray-300 rounded-lg shadow-lg hover:shadow-xl transition-shadow duration-300"
                >
                  <h2 className="text-lg font-semibold text-[#F7F7F9]">
                    {movie.movieName}
                  </h2>
                  <p className="text-sm text-gray-400 mt-2">
                    {movie.description}
                  </p>
                  <p className="text-xs text-gray-400 mt-1 font-semibold">
                    Release Date: {movie.releaseYear}
                  </p>
                </div>
              ))}
          </div>
        </div> */}
      </div>
    </ProtectedRoute>
  );
};

export default MoviePage;


