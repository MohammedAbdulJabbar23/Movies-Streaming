"use client";
import React, { useState } from "react";
import axios from "axios";
import Cookies from "js-cookie";

const CommentForm = ({
  setShowCommentForm,
  showCommentForm,
  id,
  setComments,
  comments,
}) => {
  const [commentContent, setCommentContent] = useState("");
  const token = Cookies.get("token");
  console.log(token);

  const userName = Cookies.get("userName");
  const handleAddComment = async (e) => {
    e.preventDefault();

    const token = Cookies.get("token"); // Assuming you are using cookies to store the token

    if (!token) {
      console.log("No token found");
      return; // Optionally handle the case when there is no token
    }

    try {
      const response = await axios.post(
        `http://localhost:5020/api/Comment`,
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

      if (response.status === 201 || response.status === 200) {
        const newComment = response.data;
        setShowCommentForm(!showCommentForm);
        setComments((prevComments) => [...prevComments, newComment]);
        setCommentContent("");
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
