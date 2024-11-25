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
  const userName = Cookies.get("userName");

  const handleAddComment = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post(`http://localhost:5020/api/Comment`, {
        userName: userName,
        text: commentContent,
        movieId: id,
      });

      if (response.status === 201 || response.status === 200) {
        const newComment = response.data;
        setShowCommentForm(!showCommentForm);
        setComments((prevComments) => [...prevComments, newComment]);
        setCommentContent("");
      }
    } catch (error) {
      console.log(error);
    }
  };

  return (
    <div>
      <form onSubmit={(e) => handleAddComment(e)} className="flex gap-5 items-center">
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
