import { Toaster } from "react-hot-toast";
import AppRoutes from "./routes/AppRoutes";

function App() {
  return (
    <>
      <Toaster
        position="bottom-left"
        toastOptions={{
          style: {
            marginBottom: "42px",
            marginLeft: "0px",
          },
          gutter: 10,
        }}
      />
      <AppRoutes />
    </>
  );
}

export default App;
