import "../src/index.css";
import { Route, Routes } from "react-router-dom";
import { Navbar } from "./components/Navbar.jsx";
import { Login } from "./pages/Login/Login";
import { SuperiorEvaluation } from "./pages/superior/SuperiorEvaluation";
import { SuperiorNASList } from "./pages/superior/SuperiorNASList";
import { NASPage } from "./pages/NAS/NASPage";
import { OASPage } from "./pages/OAS/OASPage";
import { OASSpecificNAS } from "./pages/OAS/OASSpecificNAS.jsx";

function App() {
  return (
    <>
      <Navbar />
      <Routes>
        <Route path="/" element={<Login />} />
        {/* Superior */}
        <Route
          path="/superior/:superiorId/evaluation/:nasId"
          element={<SuperiorEvaluation />}
        />
        <Route path="/superior/:superiorId" element={<SuperiorNASList />} />
        {/* OAS */}
        <Route path="/oas" element={<OASPage />} />
        <Route path="/oas/:nasId" element={<OASSpecificNAS />} />
        {/* NAS */}
        <Route path="/nas/:nasId" element={<NASPage />} />
      </Routes>
    </>
  );
}

export default App;
