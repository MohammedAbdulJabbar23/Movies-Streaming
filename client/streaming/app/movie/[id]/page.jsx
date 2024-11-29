"use client";
import React, { useState, useEffect } from "react";
import axios from "axios";
import Loader from "@/app/components/Loader";
import useFormattedDate from "@/app/hooks/useFormattedDate";
import ProtectedRoute from "@/app/components/ProtectedRoute";
import CommentForm from "@/app/components/CommentForm";
import Link from "next/link";

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

  return (
    <ProtectedRoute>
      <div className="relative w-full bg-gradient-to-b from-gray-900 to-black text-white overflow-hidden">
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
                  Lorem ipsum dolor sit amet consectetur adipisicing elit. Nihil
                  itaque nemo dolorem. Quo, esse voluptates soluta porro et
                  velit enim repellat, totam temporibus amet tenetur incidunt at
                  in iste id!
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

        {/* comment section */}

        <section className="pt-5 px-10 pb-6 w-[100vw]">
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
      </div>
    </ProtectedRoute>
  );
};

export default MoviePage;
