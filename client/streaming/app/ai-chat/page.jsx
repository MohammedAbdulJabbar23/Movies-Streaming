"use client"; // Ensure this runs client-side

import { useState, useEffect, useRef } from "react";
import axios from "axios";
import Link from "next/link"; // Import Link from Next.js
import "../globals.css";

const ChatPage = () => {
  const [messages, setMessages] = useState([
    { sender: "ai", text: "Welcome to Teletabiz! How can I help you today?" },
  ]);
  const [userInput, setUserInput] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const messagesEndRef = useRef(null);

  // Function to handle message submission
  const handleSendMessage = async (e) => {
    e.preventDefault();

    if (!userInput.trim()) return;

    const userMessage = { sender: "user", text: userInput };
    setMessages((prev) => [...prev, userMessage]);
    setUserInput(""); // Clear input field
    setIsLoading(true);

    try {
      const response = await axios.post(
        "http://localhost:5020/api/MovieSuggestions/suggest-movies",
        { question: userInput }
      );

      let movieSuggestions;
      if (response.data.suggestions && response.data.suggestions.length > 0) {
        // If movies are returned, map each suggestion to a clickable Link component
        movieSuggestions = response.data.suggestions.map((movie) => (
          <div key={movie.movieId} className="my-1">
            <Link
              href={`/movie/${movie.movieId}`} // Link to the movie details page
              className="cursor-pointer text-blue-500 hover:underline"
              style={{ color: "#ffffff" }} // Ensure the text color is white for visibility
            >
              - {movie.movieName} ({movie.releaseYear})
            </Link>
          </div>
        ));
      } else {
        // If no movies are returned, show a message
        movieSuggestions = (
          <div className="my-1 text-white">No movies found for your query.</div>
        );
      }

      const aiMessage = {
        sender: "ai",
        text: movieSuggestions, // Movie suggestions are JSX elements
      };

      setMessages((prev) => [...prev, aiMessage]);
    } catch (error) {
      console.error("Error fetching AI response:", error);
      setMessages((prev) => [
        ...prev,
        {
          sender: "ai",
          text: "No movies were found.",
        },
      ]);
    } finally {
      setIsLoading(false);
    }
  };

  // Scroll to the bottom whenever the messages update
  useEffect(() => {
    if (messagesEndRef.current) {
      messagesEndRef.current.scrollIntoView({ behavior: "smooth" });
    }
  }, [messages]);

  return (
    <div className="bg-[#242932] chat-container flex flex-col h-[100vh] w-[100vw] pt-24 px-24 absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2">
      {/* Chat Messages */}
      <div className="flex-grow p-4 overflow-y-auto chat-container">
        {messages.map((msg, index) => (
          <div
            key={index}
            className={`my-2 p-3 rounded-lg max-w-xs ${
              msg.sender === "ai"
                ? "bg-purple-700 text-white self-start"
                : "bg-gray-400 text-black self-end"
            }`}
            style={{
              display: "block", // Ensure the div resizes according to message length
              maxWidth: "50%", // Limit width to 50% of the container width, adjust as needed
              whiteSpace: "pre-wrap", // Allow wrapping for long messages
              wordWrap: "break-word", // Make sure long words break to fit
            }}
          >
            {/* If the message is from AI and it's an array of JSX elements, render them directly */}
            {Array.isArray(msg.text)
              ? msg.text
              : msg.text && typeof msg.text === "string"
              ? msg.text
              : null}
          </div>
        ))}
        {isLoading && (
          <div className="my-2 p-3 bg-purple-500 text-white self-start rounded-lg max-w-xs">
            Typing...
          </div>
        )}

        {/* This element ensures that the scroll always moves to the last message */}
        <div ref={messagesEndRef} />
      </div>

      {/* Input Box */}
      <form onSubmit={handleSendMessage} className="flex items-center p-4">
        <input
          type="text"
          placeholder="Type your message..."
          value={userInput}
          onChange={(e) => setUserInput(e.target.value)}
          className="flex-grow p-2 border rounded-md mr-2 bg-transparent"
        />
        <button
          type="submit" // The button is now used to trigger the form submission as well
          className="bg-purple-500 text-white px-4 py-2 rounded-md hover:bg-blue-600"
        >
          Send
        </button>
      </form>
    </div>
  );
};

export default ChatPage;
