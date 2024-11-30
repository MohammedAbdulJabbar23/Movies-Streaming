"use client";
import React, { useState, useEffect, useRef } from "react";
import axios from "axios";

const page = () => {
  const [messages, setMessages] = useState([
    { sender: "ai", text: "Welcome to Teletabiz! How can I help you today?" },
  ]);
  const [userInput, setUserInput] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const messagesEndRef = useRef(null); // Reference to scroll to the last message

  // Function to handle the message submission
  const handleSendMessage = async (e) => {
    // Prevent the form from refreshing the page
    e.preventDefault();

    if (!userInput.trim()) return;

    const userMessage = { sender: "user", text: userInput };
    setMessages((prev) => [...prev, userMessage]);
    setUserInput(""); // Clear the input field
    setIsLoading(true);

    try {
      const response = await axios.post(
        "http://localhost:5020/api/MovieSuggestions/suggest-movies",
        {
          question: userInput,
        }
      );

      console.log(response);

      // Map each movie suggestion to a new line
      const movieSuggestions = response.data.suggestions.map(
        (movie) => `- ${movie.movieName} (${movie.releaseYear})`
      );

      // Create an AI message with each movie on a separate line
      const aiMessage = {
        sender: "ai",
        text: movieSuggestions, // Now storing an array of movie suggestions
      };

      setMessages((prev) => [...prev, aiMessage]);
    } catch (error) {
      console.error("Error fetching AI response:", error);
      setMessages((prev) => [
        ...prev,
        {
          sender: "ai",
          text: "Sorry, something went wrong. Please try again later.",
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
    <div className="bg-[#242932] flex flex-col h-[100vh] w-[100vw] pt-24 px-24 absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2">
      {/* Chat Messages */}
      <div className="flex-grow p-4 overflow-y-auto">
        {messages.map((msg, index) => (
          <div
            key={index}
            className={`my-2 p-3 rounded-lg max-w-xs ${
              msg.sender === "ai"
                ? "bg-blue-500 text-white self-start"
                : "bg-gray-300 text-black self-end"
            }`}
            style={{
              display: "block", // Ensure the div resizes according to message length
              maxWidth: "50%", // Limit width to 80% of the container width, adjust as needed
              whiteSpace: "pre-wrap", // Allow wrapping for long messages (if needed)
              wordWrap: "break-word", // Make sure long words break to fit
            }}
          >
            {/* If the message is from AI and it's an array, render each item as a separate line */}
            {Array.isArray(msg.text)
              ? msg.text.map((line, i) => <p key={i}>{line}</p>)
              : // For non-array messages (initial message), simply display the text
                msg.text}
          </div>
        ))}
        {isLoading && (
          <div className="my-2 p-3 bg-blue-300 text-white self-start rounded-lg max-w-xs">
            Typing...
          </div>
        )}

        {/* This element ensures that the scroll always moves to the last message */}
        <div ref={messagesEndRef} />
      </div>

      {/* Input Box */}
      <form
        onSubmit={handleSendMessage}
        className="flex items-center p-4 border-t"
      >
        <input
          type="text"
          placeholder="Type your message..."
          value={userInput}
          onChange={(e) => setUserInput(e.target.value)}
          className="flex-grow p-2 border rounded-md mr-2 bg-transparent"
        />
        <button
          type="submit" // The button is now used to trigger the form submission as well
          className="bg-blue-500 text-white px-4 py-2 rounded-md hover:bg-blue-600"
        >
          Send
        </button>
      </form>
    </div>
  );
};

export default page;
