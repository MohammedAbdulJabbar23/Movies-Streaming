import ProtectedRoute from "./components/ProtectedRoute";
import Featured from "./components/Featured";
import Slider from "./components/Slider";
import Movie from "./components/Movie";
import "./globals.css";

export default function Home() {
  return (
    <div>
        <Featured />
        <h2 className="text-3xl font-bold m-4 mt-7">Trending Movies</h2>
        <Movie />
    </div>
  );
}
