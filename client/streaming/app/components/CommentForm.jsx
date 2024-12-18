"use client";
import React, { useState } from "react";
import axios from "axios";
import Cookies from "js-cookie";

const CommentForm = ({
  setShowCommentForm,
  showCommentForm,
  id,
  setComments,
}) => {
  const [commentContent, setCommentContent] = useState("");

  const apiUrl = process.env.NEXT_PUBLIC_NEW_API_URL;
  const token = Cookies.get("token");

  const handleAddComment = async (e) => {
    e.preventDefault();

    if (!token) {
      console.log("No token found");
      return; // Optionally handle the case when there is no token
    }

    try {
      // POST request to create a new comment
      const postResponse = await axios.post(
        `${apiUrl}/Comment`,
        {
          text: commentContent,
          movieId: id,
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (postResponse.status === 201 || postResponse.status === 200) {
        const newCommentId = postResponse.data.id; // Extract the ID of the newly created comment

        // GET request to fetch the new comment by ID
        const getResponse = await axios.get(`${apiUrl}/Comment/${newCommentId}`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });

        if (getResponse.status === 200) {
          const fetchedComment = getResponse.data;
          console.log(fetchedComment);

          // Update the comments state with the fetched comment
          setComments((prevComments) => [...prevComments, fetchedComment]);

          // Reset form state
          setShowCommentForm(!showCommentForm);
          setCommentContent("");
        }
      }
    } catch (error) {
      console.log("API Error:", error.response || error.message);
    }
  };

  return (
    <div>
      <form
        onSubmit={(e) => handleAddComment(e)}
        className="flex gap-5 items-center"
      >
        <input
          type="text"
          className="bg-transparent border p-2 text-sm w-64"
          placeholder="add comment..."
          value={commentContent}
          onChange={(e) => setCommentContent(e.target.value)}
        />
        <button
          className="border p-2 text-sm hover:text-black hover:bg-white ease-in-out duration-200"
          type="submit"
        >
          Confirm
        </button>
        <p
          title="close"
          className="hover:text-red-500 ease-in duration-100 cursor-pointer text-xl"
          onClick={() => setShowCommentForm(!showCommentForm)}
        >
          X
        </p>
      </form>
    </div>
  );
};

export default CommentForm;
