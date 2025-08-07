package com.secom.emergency.service;

import com.secom.emergency.entity.NewTerminalRegist;
import com.secom.emergency.repository.NewTerminalRegistRepository;
import org.apache.commons.csv.CSVFormat;
import org.apache.commons.csv.CSVParser;
import org.apache.commons.csv.CSVRecord;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.multipart.MultipartFile;

import java.io.IOException;
import java.io.InputStreamReader;
import java.nio.charset.StandardCharsets;
import java.util.ArrayList;
import java.util.List;

@Service
@Transactional
public class TerminalRegistService {
    
    @Autowired
    private NewTerminalRegistRepository terminalRepository;
    
    /**
     * CSVファイルから端末データを登録
     */
    public List<String> registerTerminalsFromCsv(MultipartFile file) throws IOException {
        List<String> errorMessages = new ArrayList<>();
        
        // ファイル形式チェック
        if (!isValidCsvFile(file)) {
            errorMessages.add("CSVファイル形式ではありません。");
            return errorMessages;
        }
        
        try (CSVParser parser = new CSVParser(new InputStreamReader(file.getInputStream(), StandardCharsets.UTF_8),
                CSVFormat.DEFAULT.withFirstRecordAsHeader())) {
            
            int lineNumber = 1; // ヘッダー行を考慮
            for (CSVRecord record : parser) {
                lineNumber++;
                
                try {
                    String terminalSerialNumber = record.get(0);
                    String devicePinCode = record.get(1);
                    String productCode = record.get(2);
                    
                    // パラメータチェック
                    List<String> validationErrors = validateTerminalData(terminalSerialNumber, devicePinCode, productCode, lineNumber);
                    if (!validationErrors.isEmpty()) {
                        errorMessages.addAll(validationErrors);
                        continue;
                    }
                    
                    // 重複チェック
                    if (terminalRepository.existsByTerminalSerialNumber(terminalSerialNumber)) {
                        errorMessages.add(String.format("行 %d: 端末製造番号 %s は既に登録されています。", lineNumber, terminalSerialNumber));
                        continue;
                    }
                    
                    // データベース登録
                    NewTerminalRegist terminal = new NewTerminalRegist(terminalSerialNumber, devicePinCode, productCode);
                    terminalRepository.save(terminal);
                    
                } catch (Exception e) {
                    errorMessages.add(String.format("行 %d: データ処理エラー - %s", lineNumber, e.getMessage()));
                }
            }
        }
        
        return errorMessages;
    }
    
    /**
     * 端末データのバリデーション
     */
    private List<String> validateTerminalData(String terminalSerialNumber, String devicePinCode, String productCode, int lineNumber) {
        List<String> errors = new ArrayList<>();
        
        // 端末製造番号チェック（15桁の数字）
        if (terminalSerialNumber == null || !terminalSerialNumber.matches("\\d{15}")) {
            errors.add(String.format("行 %d: 端末製造番号は15桁の数字で入力してください。", lineNumber));
        }
        
        // 機器PINコードチェック（5桁の数字）
        if (devicePinCode == null || !devicePinCode.matches("\\d{5}")) {
            errors.add(String.format("行 %d: 機器PINコードは5桁の数字で入力してください。", lineNumber));
        }
        
        // 商品コードチェック（8桁の英数字）
        if (productCode == null || !productCode.matches("[A-Za-z0-9]{8}")) {
            errors.add(String.format("行 %d: 商品コードは8桁の英数字で入力してください。", lineNumber));
        }
        
        return errors;
    }
    
    /**
     * CSVファイル形式チェック
     */
    private boolean isValidCsvFile(MultipartFile file) {
        if (file == null || file.isEmpty()) {
            return false;
        }
        
        String filename = file.getOriginalFilename();
        return filename != null && filename.toLowerCase().endsWith(".csv");
    }
    
    /**
     * 全ての端末データを取得
     */
    @Transactional(readOnly = true)
    public List<NewTerminalRegist> getAllTerminals() {
        return terminalRepository.findAll();
    }
    
    /**
     * 端末製造番号で検索
     */
    @Transactional(readOnly = true)
    public NewTerminalRegist findByTerminalSerialNumber(String terminalSerialNumber) {
        return terminalRepository.findByTerminalSerialNumber(terminalSerialNumber).orElse(null);
    }
}