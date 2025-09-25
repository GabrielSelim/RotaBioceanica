import Navbar from './components/Navbar';
import Footer from './footer/Footer';

export default function Layout({ children, language, setLanguage }) {
    return (
        <>
            <Navbar language={language} setLanguage={setLanguage} />
            <div className="main-content">{children}</div>
            <Footer language={language} />
        </>
    );
}